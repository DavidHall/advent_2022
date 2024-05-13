#r "nuget: unquote"

open Swensen.Unquote
open System.IO
open System.Collections.Generic
open System
open System.Linq

// let input = 
//     [|
//     "Sabqponm"
//     "abcryxxl"
//     "accszExk"
//     "acctuvwj"
//     "abdefghi"
//      |] 
//      :> seq<string>

let path = "./input.txt"

let input = File.ReadAllLines path

type Paths = (int * int) list list

let grid =
    input
    |> Seq.map List.ofSeq
    |> array2D

let visited = Array2D.create (grid.GetLength 0) (grid.GetLength 1) false 

let findStart (arr: char [,]) =
    let rec aux x y acc =
        if y >= arr.GetLength 1 then acc
        elif x >= arr.GetLength 0 then aux 0 (y + 1) acc
        elif arr[x,y] = 'S' || arr[x,y] = 'a' then aux (x + 1) y ([(x,y)] :: acc)
        else aux (x + 1) y acc
    aux 0 0 []

let isValidStep (here: char) (there: char) =
    let there' =
        match there with
        | 'E' -> 'z'
        | _ -> there

    if here = 'S' then true
    elif (int there') - (int here) <= 1 then true
    else false

let getNeighbours' (arr: char[,]) (p: int * int) =
    let x, y = p
    let here = arr[x, y]
    let ns = Array.create 4 None 
    if not (x = 0) && isValidStep here arr[x - 1, y] then
        ns[0] <- Some (x - 1, y)
    else
        ()

    if not (y = 0) && isValidStep here arr[x, y - 1] then
        ns[1] <- Some (x, y - 1)
    else
        ()

    if not (x >= (arr.GetLength 0) - 1) && isValidStep here arr[x + 1, y] then
        ns[2] <- Some (x + 1, y)
    else
        ()

    if not (y >= (arr.GetLength 1) - 1) && isValidStep here arr[x, y + 1] then
        ns[3] <- Some (x, y + 1)
    else
         ()

    ns
    |> Array.toList
    |> List.filter(fun x -> x.IsSome)
    |> List.map(fun x -> x.Value)

let getNeighbours = getNeighbours' grid


let haveFoundEnd' (grid: char[,])  (paths: Paths) =     
    paths
        |> List.exists(fun l -> grid[(fst l[0]), snd(l[0])] = 'E')

let haveFoundEnd = haveFoundEnd' grid

let expandPath (path: (int * int) list): Paths =
    let front = path[0]
    let neighbours = getNeighbours front

    let unvisitedNeighbours = 
        neighbours
        |> List.filter(fun n -> not (path |> List.contains(n)))

    // let unvisitedNeighbours = 
    //     neighbours
    //     |> List.filter(fun n -> not visited[fst n, snd n])

    unvisitedNeighbours
    |> List.iter(fun p -> visited[fst p, snd p] <- true)

   //printfn "visited %A" visited

    if unvisitedNeighbours.Length = 0 then
        []
    else
        unvisitedNeighbours
        |> List.map(fun n -> n :: path)
    
 
let finderStep (paths: Paths) : Paths =  
    let r = 
        paths
        |> List.map(expandPath)  
        |> List.concat  
    
    //printfn "%A" visited

    // deduplicate list    
    let unique = 
        r
        |> List.groupBy(fun r -> r[0])
        |> List.map(fun g -> (snd g)[0])    

    unique

let finder (start: Paths) = 
    let rec aux (paths: Paths) =
        if haveFoundEnd paths then
            paths
        else            
            aux (finderStep paths)

    aux start


let start = findStart grid

start
|> List.iter(fun p -> p |> List.iter(fun p -> visited[fst p, snd p] <- true))

//printfn "%A" start

let result =
    finder start
    // |> List.map(finder)
    // |> List.map(fun l -> l|> List.map(List.length) |> List.max)
    // |> List.min
    |> List.map(List.length)
    |> List.max

//let neighbours = getNeighbours start

//printfn "%A" result

//printfn "hi"
printfn "%A" (result - 1)
