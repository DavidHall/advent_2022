#r "nuget: unquote"

open Swensen.Unquote
open System.IO
open System.Collections.Generic
open System
open System.Linq

// let input = 
//      [| 
//          "noop"
//          "addx 3"
//          "addx -5"
//      |] 
//      :> seq<string>

let path = "./input.txt"
//let path = "./sample.txt"

let input = File.ReadAllLines path



type Instruction =
    | Noop
    | Addx of int

let instToStr inst =
    match inst with
    | Noop -> "noop"
    | Addx x -> $"addx {x}"

let parseLine line =
    match line |> Seq.toList with
    | 'n' :: 'o' :: 'o' :: 'p' :: _ -> Noop
    | 'a' :: 'd' :: 'd' :: 'x' :: ' ' :: i -> Addx (int (i |> Seq.map string |> String.concat ""))
    | _ -> failwith "unknown instruction"

test <@ parseLine "noop" = Noop @>    
test <@ parseLine "addx 3" = Addx 3 @>
test <@ parseLine "addx -5" = Addx -5 @>

let instructions = 
    input
    |> Seq.map parseLine
    |> Seq.toList

let instructionFolder (acc: (int * int) list * int * int) inst =
    let (insts, register, cycle) = acc
    
    match inst with
        | Noop -> ((register, cycle) :: insts, register, cycle + 1)
        | Addx x -> ((register + x, cycle + 1) :: (register, cycle) :: insts, register + x, cycle + 2)

let instructionProcessor (instructions: Instruction list) =
    let result = 
        instructions
        |> List.fold instructionFolder ([], 1, 1)

    let r, _, _ = result

    r |> List.rev

let result = instructionProcessor instructions

let screen = Array2D.create<string> 6 40 "."

let getPixelValue i j  (locations : (int * int) list)  =
    let cycle = (i + (j * 40))

    if cycle = 0 then
        "#"
    else
        let r, c = locations[cycle - 1]

        if r = i || r - 1 = i || r + 1 = i then
            "#"
        else
            "."

for j in 0..(5) do 
    for i in 0..(39) do     
        screen.[j, i] <- getPixelValue i j result

let getRow (screen : string array2d) idx =
    screen[idx, 0..49] |> String.concat ""

let getRow' = getRow screen

let drawn = 
    [0..5] 
    |> List.map getRow'


printfn "%A" drawn
   
