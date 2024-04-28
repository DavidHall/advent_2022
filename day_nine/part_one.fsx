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

let move (state: HeadTail) (direction : Direction) =
    let t = fst state
    let h = snd state 

    let h' =
        match direction with
        | U -> (fst h, (snd h + 1))
        | D -> (fst h, (snd h - 1))
        | L -> ((fst h - 1), snd h)
        | R -> ((fst h + 1), snd h)

    if tailMustMove (t, h') then  
        match direction with
        | U -> ((fst h', snd h' - 1), h')
        | D -> ((fst h', snd h' + 1), h')
        | L -> ((fst h' + 1, snd h'), h')
        | R -> ((fst h' - 1, snd h'), h')
    else 
        (t, h')

let folder (acc: HeadTail * Point list) (d: Direction): HeadTail * Point list =  
    let (p, arr) = acc
    let t = fst p
    
    ((move p d), t :: arr)


let result = 
    directions
        |> Array.fold folder (((1, 1), (1 ,1)), []) 

let dist =
    (snd result)
    |> List.distinct
    |> List.length

printfn "%A" dist

// let input = File.ReadAllLines(path)

// let result = 
//     treesToCheck
//     |> List.map isVisible'
//     |> List.ter(fun x -> x)
//     |> List.length
//

