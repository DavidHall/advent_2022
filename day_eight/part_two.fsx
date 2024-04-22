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

let outerCount = 4 * dimension - 4

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
   
    // count from left
    let visibleLeft' =
        left
        |> Array.rev
        |> Array.tryFindIndex(fun f -> f >= height)

    let visibleLeft = 
        match visibleLeft' with
        | Some i -> i + 1
        | None -> left.Length
    
   
    let visibleRight' =
        right
        |> Array.tryFindIndex(fun f -> f >= height)

    let visibleRight =
        match visibleRight' with
        | Some i -> i + 1
        | None -> right.Length

    let visibleUp' =
        up
        |> Array.rev
        |> Array.tryFindIndex(fun f -> f >= height)

    let visibleUp =
        match visibleUp' with
        | Some i -> i + 1
        | None -> up.Length

    let visibleDown' =
        down
        |> Array.tryFindIndex(fun f -> f >= height)

    let visibleDown =
        match visibleDown' with
        | Some i -> i + 1
        | None -> down.Length

    visibleUp * visibleDown * visibleLeft * visibleRight


let countVisible' = countVisible grid    
let result = 
    treesToCheck
    |> List.map countVisible'
    |> List.max

printfn "%A" (result)

