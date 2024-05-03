#r "nuget: unquote"

open Swensen.Unquote
open System.IO
open System.Collections.Generic
open System
open System.Linq
open System.Text.RegularExpressions

// let input = 
//      [| 
//          "Monkey 0:"
//          "  Starting items: 79, 98"
//          "  Operation: new = old * 19"
//          "  Test: divisible by 23"
//          "    If true: throw to monkey 2"
//          "    If false: throw to monkey 3"
//          ""
//          "Monkey 1:"
//          "  Starting items: 54, 65, 75, 74"
//          "  Operation: new = old + 6"
//          "  Test: divisible by 19"
//          "    If true: throw to monkey 2"
//          "    If false: throw to monkey 0"
//          ""
//          "Monkey 2:"
//          "  Starting items: 79, 60, 97"
//          "  Operation: new = old * old"
//          "  Test: divisible by 13"
//          "    If true: throw to monkey 1"
//          "    If false: throw to monkey 3"
//          ""
//          "Monkey 3:"
//          "  Starting items: 74"
//          "  Operation: new = old + 3"
//          "  Test: divisible by 17"
//          "    If true: throw to monkey 0"
//          "    If false: throw to monkey 1"
//      |] 
//      :> seq<string>


let path = "./input.txt"

let input = File.ReadAllLines path

type Operation = int64 -> int64

let multiply a b = a * b
let plus a b = a + b

let square a = a * a

type Monkey = { id: int; items: int64 list; ifTrue: int; ifFalse: int; test: int64; operation: Operation; inspections: int64 }

let parseStartingItems (items: string) =
    let parts = items.Split [|','|]
 
    parts 
    |> Array.map((fun x -> x.Trim()) >> int64)
    |> Array.toList

let (|Operation|_|) input =
    let mSquare = Regex.Match(input,"  Operation: new = old \* old")
    let mPlus = Regex.Match(input,"  Operation: new = old \+ ([0-9]+)")
    let mMultiply = Regex.Match(input,".*Operation: new = old \* ([0-9]+)")    

    if (mSquare.Success) then 
        Some (square) 
    elif (mPlus.Success) then
        Some (plus (int64 mPlus.Groups.[1].Value))
    elif (mMultiply.Success) then
        Some (multiply (int64 mMultiply.Groups.[1].Value))        
    else None

let (|MonkeyP|_|) input =
   let m = Regex.Match(input,"Monkey ([0-9]+).*")
   if (m.Success) then Some (int m.Groups.[1].Value) else None

let (|Items|_|) input =
   let m = Regex.Match(input,".*Starting.*:(.*)")   
   if (m.Success) then Some (parseStartingItems m.Groups.[1].Value) else None

let (|Test|_|) input =
   let m = Regex.Match(input,".*Test[:a-zA-Z\s]*([0-9]{1,2})")   
   if (m.Success) then Some (int m.Groups.[1].Value) else None

let (|IfTrue|_|) input =
   let m = Regex.Match(input,".*true.*monkey\s([0-9])")
   if (m.Success) then Some (int m.Groups.[1].Value) else None

let (|IfFalse|_|) input =
   let m = Regex.Match(input,".*false.*monkey\s([0-9])")
   if (m.Success) then Some (int m.Groups.[1].Value) else None

let rec gcd a b =
    if b = int64 0
        then abs a
    else gcd b (a % b)

let lcmSimple a b = a*b/(gcd a b)

let rec lcm = function
    | [a;b] -> lcmSimple a b
    | head::tail -> lcmSimple (head) (lcm (tail))
    | [] -> 1

let monkeyInspection' (monkeyMod: int64) (item: int64) (monkey: Monkey) =
    let worry = monkey.operation item
    let afterBored = worry % monkeyMod
    let isDivible = (afterBored % monkey.test) = 0
    if (isDivible) then
        afterBored, monkey.ifTrue
    else
        afterBored, monkey.ifFalse


let parseMonkey lines =
    let rec aux lines' (m: Monkey) = 
        match lines'  with
        | [] -> ([], m)
        | h :: rest ->        
            printfn "%A" h   
            match h with
            | MonkeyP n -> aux rest { m with id = n }
            | Items i -> aux rest { m with items = i }
            | IfTrue i -> aux rest { m with ifTrue = i }
            | IfFalse i -> aux rest { m with ifFalse = i }
            | Test i -> aux rest { m with test = i }
            | Operation o -> aux rest { m with operation = o }
            //| "" -> (rest m)
            | _ -> (rest, m) 
    
    aux lines { id = -1; items = []; ifTrue = - 1; ifFalse = -1; test = -1; operation = id; inspections = 0 }

let parseInput lines =
    let rec aux lines' result =
        match lines' with
        | [] -> result 
        | h :: rest -> 
            match h with
            | (l: string) when l.StartsWith("Monkey") ->
                let r, m = parseMonkey (h :: rest)
                aux r (m :: result) 
            | _ -> failwith "part of monkey not parsed"

    aux lines []



let monkeyFolder' (monkeyMod: int64) (accum: (int64 * int) list * Monkey list) (monkey: Monkey) =    

    let monkeyInspection =
        monkeyInspection' monkeyMod

    let acc = fst accum
    let monkeyList = snd accum

    let thrownToMonkey = 
        acc
        |> List.filter(fun t -> (snd t = monkey.id))
        |> List.map(fst)

    let acc' = 
        acc
        |> List.filter(fun t -> not ((snd t) = monkey.id))    

    let throws = 
        thrownToMonkey @ monkey.items
        |> List.map(fun i -> monkeyInspection i monkey)    

    let inspectionCount =
        throws
        |> List.length
        |> int64
        |> (+) monkey.inspections

    (throws @ acc', { monkey with inspections = inspectionCount } :: monkeyList)

let run () =
    printfn "testing..."    

    let testInput =
        [
            "Monkey 0:"
            "  Starting items: 75, 63"
            "  Operation: new = old * 3"
            "  Test: divisible by 11"
            "    If true: throw to monkey 7"
            "    If false: throw to monkey 2"
            ""
            // "Monkey 0:"
            // "  Starting items: 79, 98"
            // "  Operation: new = old * 19"
            // "  Test: divisible by 13"
            // "    If true: throw to monkey 1"
            // "    If false: throw to monkey 3"
        ]

    let multiply' a = multiply a 19 

    let expected = [ { id = 0; items = [79; 98]; ifTrue = 1; ifFalse = 3; test = 13; operation = multiply'; inspections = 0 } ]    

    let result = parseInput testInput 

    let compareMonkey a b = 
        a.id = b.id 
        && a.items = b.items
        && a.ifTrue = b.ifTrue
        && a.ifFalse = b.ifFalse
        && a.test = b.test
        && a.operation 5 = b.operation 5

    test <@ compareMonkey result[0] expected[0] @>

    printfn "done."

    let monkeys =
        input
        |> Seq.toList
        |> parseInput
        |> List.rev

    let monkeyModulo = 
        monkeys
        |> List.map(fun m -> m.test)
        |> lcm

    let getResultForMonkey' (throws: (int64 * int) list) (monkey: Monkey) =
        let selectedItems = 
            throws
            |> List.filter(fun t -> snd t = monkey.id)
            |> List.map(fst)

        { monkey with items = selectedItems }

    let monkeyFolder =
        monkeyFolder' monkeyModulo

    let processRound monkeys = 
        let roundResults = 
            monkeys
            |> List.fold monkeyFolder ([], [])

        let getResultForMonkey =
            getResultForMonkey' (fst roundResults)

        let newMonkeys = 
            roundResults 
            |> snd
            |> List.map(getResultForMonkey)
            |> List.rev

        newMonkeys

    let chaseTheMonkeys monkeys =
        let rec aux round monkeys' =
            let roundResults = processRound monkeys'
            match round with
            | 10000 -> roundResults
            | _ -> aux (round + 1) roundResults

        aux 1 monkeys

    let chaseResults = 
        chaseTheMonkeys monkeys
        |> List.map(fun m -> (m.id, m.inspections) )

    chaseResults 
    |> List.iter(fun cr -> printfn "Monkey %i inspected items %i times." (fst cr) (snd cr))    

    let r =
        chaseResults
        |> List.map snd
        |> List.sortDescending
        |> List.take 2
        |> List.fold (*) 1L

    printfn "%A" r

    ()

run ()

    


