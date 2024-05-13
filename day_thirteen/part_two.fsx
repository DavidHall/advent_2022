#r "nuget: unquote"

open Swensen.Unquote
open System.IO
open System.Collections.Generic
open System
open System.Linq

// let input =
//     [|
//         "[1,1,3,1,1]"
//         "[1,1,5,1,1]"
//         ""
//         "[[1],[2,3,4]]"
//         "[[1],4]"
//         ""
//         "[9]"
//         "[[8,7,6]]"
//         ""
//         "[[4,4],4,4]"
//         "[[4,4],4,4,4]"
//         ""
//         "[7,7,7,7]"
//         "[7,7,7]"
//         ""
//         "[]"
//         "[3]"
//         ""
//         "[[[]]]"
//         "[[]]"
//         ""
//         "[1,[2,[3,[4,[5,6,7]]]],8,9]"
//         "[1,[2,[3,[4,[5,6,0]]]],8,9]"
//     |]
//     :> seq<string>

let path = "./input.txt"


let input = File.ReadAllLines(path)

type Packet =
    | I of int 
    | L of Packet list
    with override this.ToString() =
            match this with
            | I i -> i.ToString()
            | L l -> l.ToString()

let charToInt (c: char list) : int = 
    c |> Array.ofList |> String.Concat |> int

let addToL (i: Packet) (target: Packet) =
    match target with    
    | L list -> L ((i :: (list |> List.rev)) |> List.rev)
    | _ -> failwith "expected target to be l"

let parseString (s: string) : Packet =    
    let rec walkString (cs: char list) (acc: Packet) =
    
        match cs with 
        | h1 :: h2 :: rest ->
            match h1 with
            | '[' -> 
                //printfn "about to walk %A" (h2 :: rest)
                let wsResult, newRest = walkString (h2::rest) (L [])
                //printfn "wsRes %A newRest %A" wsResult newRest
                walkString newRest (addToL wsResult acc) 
            | ']' -> acc, (h2 :: rest)
            | ',' -> walkString (h2 :: rest) acc
            | _ ->            
                match h1, h2 with
                | _, ',' -> walkString rest (addToL (I (charToInt [h1])) acc)
                | _, ']' -> (addToL (I (charToInt [h1])) acc), rest                          
                | _, _ -> walkString rest (addToL (I (charToInt [h1; h2])) acc)                        
        | [i] ->             
            match i with
            | ']' -> acc, []
            | _ -> (addToL (I (charToInt [i])) acc), []
        | [] -> acc, []

    // strip the first and last brackets
    let input = 
        s |> Seq.toList
        |> List.tail
        |> List.rev
        |> List.tail
        |> List.rev    

    fst (walkString (input) (L []))    

let parsePackets lines =
    let rec aux lines' acc =
        match lines' with        
        | h1 :: h2 :: h3 :: rest -> aux rest ((parseString h1, parseString h2) :: acc)
        | [h1; h2] -> ((parseString h1, parseString h2) :: acc)
        | [_] -> acc
        | [] -> acc

    (aux lines [])
    |> List.rev


let rec comparePacket (left, right) =            
    match left, right with
    | I l, I r ->        
        match l, r with
        | l, r when l < r -> Some true
        | l, r when l > r -> Some false
        | _, _ -> None
    | L _, L _ -> comparePacketList left right
    | L _, I r -> comparePacketList left (L [I r])
    | I l, L _ -> comparePacketList (L [I l]) right

and comparePacketList (left : Packet) (right : Packet) =
    let rec walkList (l1: Packet list) (l2: Packet list) =    
        //printfn "  - Compare %A vs %A" l1 l2                  
        match l1, l2 with
        | h1 :: rest1, h2 :: rest2 -> 
            let result = comparePacket (h1, h2)
            match result with
            | None -> walkList rest1 rest2
            | Some _ -> result
        | [], [] -> None 
        | [], _ -> Some true
        | _, [] -> Some false

    match left, right with
    | I _, _ -> failwith "compare list should not receive int"
    | _, I _ -> failwith "compare list should not receive int"
    | L l1, L l2 -> walkList l1 l2

let compareDecorator i (left: Packet, right: Packet) =
    printfn "== Pair %i ==" (i + 1)
    printfn "- Compare %A vs %A" left right
    let r = comparePacket (left, right)
    //printfn "a result: \n left: \n %A \n right \n %A \n%A" left right r
    
    //printfn "result is %A" r.Value
    if r.Value then
        printfn "  - Left side is smaller, so inputs are in right order"
    else
        printfn "  - Right side is smaller, so inputs are not in right order"
    r

let compareForSort (left: Packet) (right: Packet)  =
    let r = comparePacket (left, right)

    match r with
    | Some true -> -1
    | Some false -> 1
    | None -> 0

let packets =
    input
    |> Seq.toList
    |> List.filter(fun l -> not (l = ""))
    |> List.map parseString
    |> List.append([(L [L [I 2]]); (L [ L [ I 6]])])
    |> List.sortWith compareForSort
    
let idx1 = 
    packets
    |> List.findIndex(fun i -> i = L [L [I 2]])
    |> (+) 1

let idx2 = 
    packets
    |> List.findIndex(fun i -> i = L [L [I 6]])  
    |> (+) 1  

printfn "%i" (idx1 * idx2)

