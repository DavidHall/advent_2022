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
// let input = File.ReadAllLines(path)

let instructions = 
    input
    |> Seq.map parseLine
    |> Seq.toList

let instructionFolder (acc: (int * int * string) list * int * int) inst =
    let (insts, register, cycle) = acc
    
    match inst with
        | Noop -> ((register, cycle, instToStr inst) :: insts, register, cycle + 1)
        | Addx x -> ((register + x, cycle + 1, instToStr inst) :: (register, cycle, instToStr inst) :: insts, register + x, cycle + 2)

let instructionProcessor (instructions: Instruction list) =
    let result = 
        instructions
        |> List.fold instructionFolder ([], 1, 1)

    let r, _, _ = result

    r |> List.rev

let result = instructionProcessor instructions

let interesting = result[18] :: result[58] :: result[98] :: result[138] :: result[178] :: result[218] :: []

let tmp =
    interesting
    |> List.map(fun (register, cycle, _) -> register * (cycle + 1))
    |> List.sum


//result |> List.iter(fun x -> printfn "%A" x)

//printfn "%A" result

printfn "%A" tmp      
