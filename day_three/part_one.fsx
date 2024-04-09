#r "nuget: unquote"

open Swensen.Unquote
open System.IO
open System.Collections.Generic
open System
open System.Linq

// let input = 
//     [| 
//         "vJrwpWtwJgWrhcsFMMfFFhFp"
//         "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL"
//         "PmmdzqPrVvPwwTWBwg"
//         "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn"
//         "ttgJtRGJQctTZtZT"
//         "CrZsJsPPZsGzwwsLwLmpwMDw"
//     |] 
//     :> seq<string>

let path = "./input.txt"

let input = File.ReadAllLines(path)

let findPriority(len: int, line: char array) =
    let l = len/2 - 1
    let compartmentA = line[0..l]
    let compartmentB = line[len/2..]
    let inter = compartmentA.Intersect(compartmentB).ToList () 
    match inter[0] with
    | x when int x > 96 -> int (System.Char.ToUpper(x)) - 64
    | x -> int x - 38


let tmp = 
    input 
    |> Seq.map(fun line -> (line.Length, line.ToCharArray ()))
    |> Seq.map(findPriority)
    |> Seq.sum

printfn "%A" tmp
