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

let charToAlphaIndex x =
    match x with
    | x when int x > 96 -> int (System.Char.ToUpper(x)) - 64
    | x -> int x - 38

let window(seq: char array list) =
    let rec aux seq ret =
        match seq with 
        | [] -> ret
        | h1 :: h2 :: h3 :: rest -> aux rest (([| h1 ; h2 ; h3 ;|]) :: ret)
        | _ -> failwith "Unexpected number of items in main list"

    aux seq []

let result = 
    input
    |> Seq.map(fun line -> line.ToCharArray ())
    |> Seq.toList
    |> window 
    |> Seq.map(Seq.reduce(fun a b -> a.Intersect(b).ToArray ()))
    |> Seq.map(fun x -> x[0])
    |> Seq.map(charToAlphaIndex)
    |> Seq.sum

printfn "%A" result
