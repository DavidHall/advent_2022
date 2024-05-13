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

//[<StructuredFormatDisplay("{AsString}")>]
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
        //printfn "  - Compare %A vs %A" l r
        printfn " - Compare %O vs %O" left right
        match l, r with
        | l, r when l < r -> Some true
        | l, r when l > r -> Some false
        | _, _ -> None
    | L _, L _ -> 
        printfn " - Compare %A vs %A" left right
        comparePacketList left right
    | L _, I r -> 
        printfn " - Mixed types; convert right to [%i] and retry comparison" r
        comparePacketList left (L [I r])
    | I l, L _ -> 
        printfn " - Mixed types; convert left to [%i] and retry comparison" l
        comparePacketList (L [I l]) right

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
        | [], _ ->
            printfn "    - Left ran out of items, so inputs are in the right order " 
            Some true
        | _, [] -> 
            printfn "    - Right ran out of items, so inputs are not in the right order " 
            Some false
        //| _ :: _, [] -> Some false
        //| [], _ :: _ -> Some true

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



////

let packets =
    input
    |> Seq.toList
    |> parsePackets

let result = 
    packets 
    |> List.mapi (fun i l -> compareDecorator i l)
    |> List.mapi (fun i l -> 
        match l with
        | Some true -> i + 1
        | _ -> 0
    )
    |> List.sum

printfn "%A" result

let comparePacket' l r =
    comparePacket (parseString l, parseString r)

let partOne () =
    printfn "running part 1"

//partOne ()

// comparePacket (parseString "[[5,[1],9,[[7,10,5],8,[8,7,2],[6,1]]]]",
//  parseString "[[[[1,6],6,7],[],1,[],[[8,4,4,0,4],9]]]")

let examine () =
    printfn "examinations"

    // let result = comparePacket' "[8]" "[[8]]"
    // printfn "%A" result

    //[[8],[[2,7,2,[0,3,8]],[[6,3,8],3,[0],[3,6,1,4,3],6],10,6,[0]],[7],[[],[[],2,[0]]],[4,9,[[6,7,6]],7]]
    //[[[[8]]],[[2,[3,4,0,1,6],10,[8],[]],[[7,9],4,10],[[6,8],[],6,[],[5,7,6]],[[6,7,10,5,2]],3],[2,3,2,[[10]]],[8,2,[9,8,[1,0,2]]]]
    // let result3 = comparePacket' "[[8],[]]" "[[[[8]]],[]]"
    // printfn "what happen %A" result3

    // let tmp = parseString "[[[[8]]],[]]"

    // printfn "the string %O" tmp

    // test <@ parseString "[[[8]]]" = L [ L [ L [ I 8 ] ] ] @>

    // test <@ parseString "[[[[8]]]]" = L [ L [ L [ L [ I 8 ] ] ] ] @>

    // test <@ parseString "[[[8]],[]]" = L [ L [ L [ I 8 ] ]; L [] ] @>

    test <@ parseString "[[[[8]]],[]]" = L [ L [ L [ L [ I 8 ] ]]; L [] ] @>

    // let result2 = comparePacket' "[[8],[[2,7,2,[0,3,8]],[[6,3,8],3,[0],[3,6,1,4,3],6],10,6,[0]],[7],[[],[[],2,[0]]],[4,9,[[6,7,6]],7]]" "[[[[8]]],[[2,[3,4,0,1,6],10,[8],[]],[[7,9],4,10],[[6,8],[],6,[],[5,7,6]],[[6,7,10,5,2]],3],[2,3,2,[[10]]],[8,2,[9,8,[1,0,2]]]]"

    // printfn "this is suspect %A" result2



//examine ()

let run () =

    printfn "testing..."

    test <@ parseString "[7]" = L [ I(7) ] @>

    test <@ parseString "[7,8]" = L [ I 7; I 8 ] @>

    test <@ parseString "[7,10]" = L [ I 7; I 10 ] @>

    test <@ parseString "[7,10,13]" = L [ I 7; I 10; I 13 ] @>

    test <@ parseString "[[1],[2,3,4]]" = L [ L [ I 1 ]; L [ I 2; I 3; I 4  ] ] @>

    test <@ parseString "[[]]" = L [ L [ ] ] @>

    test <@ parseString "[[[]]]" = L [ L [ L [] ] ] @>


    test <@ comparePacket ((I 1), (I 2)) = Some true @>
    test <@ comparePacket ((I 1), (I 1)) = None @>
    test <@ comparePacket ((I 2), (I 1)) = Some false @>

    test <@ comparePacket ((L [ I 1 ]), (L [ I 2 ])) = Some true @>
    test <@ comparePacket ((L [ I 1 ]), (L [ I 1 ])) = None @>
    test <@ comparePacket ((L [ I 2 ]), (L [ I 1 ])) = Some false @>

    test <@ comparePacket ((L [I 1; I 1; I 3; I 1; I 1]), (L [I 1; I 1; I 5; I 1; I 1])) = Some true  @>

    test <@ comparePacket ((L [I 6; I 9; I 3; I 1]), (L [])) = Some false @>

    test <@ comparePacket (parseString "[[[1]],1]" , parseString "[[1],2]") = Some true @>

    test <@ comparePacket (parseString "[[[1]],2]", parseString "[[1],1]") = Some false @>

    test <@ comparePacket ( parseString "[[1],1]", parseString "[[[1]],2]") = Some true @>


    test <@ comparePacket (parseString "[[1],2]", parseString "[[[1]],1]") = Some false @>




    test <@ comparePacket (parseString "[[1],[2,3,4]]", parseString "[[1],4]") = Some true @>



    test <@ comparePacket (parseString "[[1],4]", parseString "[[1],[2,3,4]]") = Some false @>

    // [1] [2] true
    // [1] [1] none
    // [2] [1] false
    test <@ comparePacket' "[1]" "[2]" = Some true @>
    test <@ comparePacket' "[1]" "[1]" = None @>
    test <@ comparePacket' "[2]" "[1]" = Some false @>

    // [] [] none
    // [[]] [[]] none
    test <@ comparePacket' "[]" "[]" = None @>
    test <@ comparePacket' "[[]]" "[[]]" = None @>

    // [[1]] [[]] false
    // [[]] [[1]] true
    test <@ comparePacket' "[[1]]" "[[]]" = Some false @>
    test <@ comparePacket' "[[]]" "[[1]]" = Some true @>

    // [[1, 2, 3]] [[1, 2]] false
    // [[1, 2, 3]] [[2, 2]] frue
    test <@ comparePacket' "[[1, 2, 3]]" "[[1, 2]]" = Some false @>
    test <@ comparePacket' "[[1, 2, 3]]" "[[2, 2]]" = Some true @>

    test <@ comparePacket' "[[10]]" "[[9, 1]]" = Some false @>
    test <@ comparePacket' "[[10]]" "[[10, 1]]" = Some true @>

    test <@ comparePacket' "[[2], 3]" "[[4], 1]]" = Some true @>

    //

    test <@ comparePacket' "[[2],[7]]" "[[2, 6]]" = Some true @>
    test <@ comparePacket' "[[],[2, 7]]" "[[2],[6]]" = Some true @>
    test <@ comparePacket' "[2, 7]" "[2,[6]]" = Some false @>


    printfn "done"

//run ()

