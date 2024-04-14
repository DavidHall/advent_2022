#r "nuget: unquote"

open Swensen.Unquote
open System.IO
open System.Collections.Generic
open System
open System.Linq

// let input = 
//     [| 
//         "2-4,6-8"
//         "2-3,4-5"
//         "5-7,7-9"
//         "2-8,3-7"
//         "6-6,4-6"
//         "2-6,4-8"
//     |] 
//     :> seq<string>

let path = "./input.txt"

let input = File.ReadAllLines(path)

type SectionAssignment = { Start: int; End: int }

let getAssignments (line: string) =
    let split = line.Split [|','|]
    let assignments = 
        split 
        |> Array.map(fun (a: string) ->
            let s = a.Split [|'-'|]
            { Start = int s[0]; End = int s[1] })
    assignments


let assignmentsOverlap (assignments) =
    match assignments with
    | [|a; b|] when a.Start <= b.End && b.Start <= a.End -> true
    | _ -> false

let result = 
    input 
    |> Seq.map(getAssignments)
    |> Seq.map(assignmentsOverlap)
    |> Seq.countBy(fun x -> x)



printfn "%A" result 
