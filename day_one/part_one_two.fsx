#r "nuget: Unquote"

open Swensen.Unquote
open System.IO
open System.Collections.Generic
open System
open System.Linq

// let input = [| 
//         "1000"
//     "2000"
//     "3000"
//     ""
//     "4000"
//     ""
//     "5000"
//     "6000"
//     ""
//     "7000"
//     "8000"
//     "9000"
//     ""
//     "10000"
//     |]
//     :> seq<string>


let path = "./input.txt"

let input = File.ReadAllLines(path)

// first group all the lines, splitting by empthy lines
let groupElves lines =
    let rec aux lines elf elves =
        match lines with
        | [] -> elves
        | head :: tail -> 
            match head with
            | "" -> aux tail [] (elf :: elves)
            | x -> aux tail ((int x) :: elf) elves

    aux lines [] []

let grouped = groupElves (Seq.toList input) 

let result_one = grouped |> List.map (List.sum) |> List.max

let result_two = grouped |> List.map (List.sum) |> List.sortDescending |> (List.take 3) |> List.sum

printfn "%A" result_one
printfn "%A" result_two
