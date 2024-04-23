#r "nuget: unquote"

open Swensen.Unquote
open System.IO
open System.Collections.Generic
open System
open System.Linq

// let input = 
//     [| 
//         "30373"
//         "25512"
//         "65332"
//         "33549"
//         "35390"
//     |] 
//     :> seq<string>

let path = "./input.txt"

let input = File.ReadAllLines(path)

let grid =
    (input |> Seq.map List.ofSeq)
    |> array2D
    |> Array2D.map (string>>int)

let dimension = grid |> Array2D.length1

let treesToCheck = 
    [for i in 1..(dimension - 2) do 
        for j in 1..(dimension - 2) -> (i, j)
    ]

let countVisible (grid: int array2d) (tree: int * int)  =
    let height = grid[fst tree, snd tree]

    let (i, j) = tree

    let left = grid[i, 0 .. j - 1]
    let right = grid[i, j + 1 .. (dimension)]
    let up = grid[0 .. i - 1, j]
    let down = grid[i + 1 .. (dimension), j]
   
    let directions = [ (left |> Array.rev); right; (up |> Array.rev); down  ]

    let result = 
        directions
        |> List.map(fun a -> (a |> Array.tryFindIndex(fun f -> f >= height), a.Length))
        |> List.map(fun x -> 
            match x with
            | (Some i, _) -> i + 1
            | (None, l) -> l
        )
        |> List.fold (*) 1

    result

let countVisible' = countVisible grid    
let result = 
    treesToCheck
    |> List.map countVisible'
    |> List.max

test <@ result = 335580 @>