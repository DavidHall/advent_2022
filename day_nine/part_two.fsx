#r "nuget: unquote"

open Swensen.Unquote
open System.IO
open System.Collections.Generic
open System
open System.Linq

// let input = 
//     [| 
//         "R 4"
//         "U 4"
//         "L 3"
//         "D 1"
//         "R 4"
//         "D 1"
//         "L 5"
//         "R 2"
//     |] 
//     :> seq<string>

// let input = 
//     [| 
//         "R 5"
//         "U 8"
//         "L 8"
//         "D 3"
//         "R 17"
//         "D 10"
//         "L 25"
//         "U 20"
//     |] 
//     :> seq<string>



let path = "./input.txt"

let input = File.ReadAllLines(path)

type Direction =
    | U 
    | D
    | L
    | R

type Point = int * int
type HeadTail = Point * Point

let parseInput (input: string) =
    let parts = input.Split [|' '|]

    let direction = 
        match parts[0] with
        | "U" -> U
        | "D" -> D
        | "L" -> L
        | "R" -> R
        | _ -> failwith "Unknown direction"

    Array.create (int parts[1]) direction

let directions = 
    input
    |> Seq.map parseInput
    |> Array.concat

let tailMustMove (state : HeadTail) =
    let t = fst state
    let h = snd state 

    if abs (fst t - fst h) > 1 || abs (snd t - snd h) > 1 then
        true
    else
        false

let moveTail t h' =
    let hx = fst h'
    let hy = snd h'
    let tx = fst t
    let ty = snd t

    let deltax =
        match hx - tx with
        | x when x = 0 -> 0
        | x when x > 0 -> 1
        | x when x < 0 -> -1 
        | _ -> failwith "this should not be possible"
    
    let deltay =
        match hy - ty with
        | y when y = 0 -> 0
        | y when y > 0 -> 1
        | y when y < 0 -> -1 
        | _ -> failwith "this should not be possible"
    
    (tx + deltax, ty + deltay)      

let moveHead (head: Point) (direction: Direction) =    
    match direction with
    | U -> (fst head, (snd head + 1))
    | D -> (fst head, (snd head - 1))
    | L -> ((fst head - 1), snd head)
    | R -> ((fst head + 1), snd head)

let foldPoints (acc : Point list) (point: Point) =
    let h = acc[0]

    if tailMustMove (point, h) then  
        (moveTail point h) :: acc
    else 
        point :: acc

let folder (acc: Point list * Point list) (d: Direction) =  
    let (ps, arr) = acc
    let t = ps[9]
    
    let newHead = moveHead ps[0] d
    let pointsToFold = ps[1..]

    let newPoints = 
        pointsToFold
        |> List.fold foldPoints [newHead] 
        |> List.rev

    (newPoints, t :: arr)

let knots: Point list = [ 
    (1, 1); // H
    (1, 1); // 1
    (1, 1); // 2
    (1, 1); // 3
    (1, 1); // 4
    (1, 1); // 5
    (1, 1); // 6
    (1, 1); // 7
    (1, 1); // 8
    (1, 1) // 9
 ]

let result = 
    directions
        |> Array.fold folder (knots, [])
        |> snd
        |> List.distinct
        |> List.length

printfn "%A" result

let testHead = (1, 3)
let testTail = (1, 1)

type TestCase = { Head: Point; Tail: Point; Expected: Point; Name: string }

let testCases = [
    { Head = (1, 3); Tail = (1, 1); Expected = (1, 2); Name = "Head is Up" };
    { Head = (1, 3); Tail = (1, 5); Expected = (1, 4); Name = "Head is Down" };
    { Head = (3, 3); Tail = (1, 1); Expected = (2, 2); Name = "Head is diag Up Right" };
    { Head = (2, 3); Tail = (1, 1); Expected = (2, 2); Name = "Head is diag Up Right 2" };
    { Head = (3, 2); Tail = (1, 1); Expected = (2, 2); Name = "Head is diag Up Right 3" };
]

printfn "testing..."

testCases 
|> List.iter(fun tc ->
    printfn "%s" tc.Name
    test <@ moveTail testTail testHead = (1, 2)  @>
)

printfn "done"


