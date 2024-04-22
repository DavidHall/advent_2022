#r "nuget: unquote"

open Swensen.Unquote
open System.IO
open System.Collections.Generic
open System
open System.Linq

// let input = 
    // [| 
    //     "30373"
    //     "25512"
    //     "65332"
    //     "33549"
    //     "35390"
    // |] 
    // :> seq<string>

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


let isVisible (grid: int array2d) (tree: int * int)  =
    let height = grid[fst tree, snd tree]

    let (i, j) = tree

    let left = grid[i, 0 .. j - 1]
    let right = grid[i, j + 1 .. (dimension)]
    let up = grid[0 .. i - 1, j]
    let down = grid[i + 1 .. (dimension), j]
    
    let visibleFromLeft = not (
        left 
        |> Array.exists (fun x -> (x >= height)))

    let visibleFromRight = not (
        right 
        |> Array.exists (fun x -> (x >= height)))

    let visibleFromUp = not (
        up 
        |> Array.exists (fun x -> (x >= height)))

    let visibleFromDown = not (
        down 
        |> Array.exists (fun x -> (x >= height)))

    (visibleFromLeft || visibleFromRight || visibleFromUp || visibleFromDown)

let isVisible' = isVisible grid    
let result = 
    treesToCheck
    |> List.map isVisible'
    |> List.filter(fun x -> x)
    |> List.length

printfn "%A" (result + outerCount)
