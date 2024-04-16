#r "nuget: unquote"

open Swensen.Unquote
open System.IO
open System.Collections.Generic
open System
open System.Linq

// let input = 
//     [| 
//         "bvwbjplbgvbhsrlpgdmjqwftvncz"
//         "nppdvjthqldpwncqszvftbrmjlhg"
//         "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg"
//         "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw"
//     |] 
//     :> seq<string>

let path = "./input.txt"

let input = File.ReadAllLines(path)

let removeFirst s =
    match s with
    | h :: rest -> rest
    | [] -> failwith "Received empty"

let getPacketMarker (line : string) =
    let chars = line |> Seq.toList

    let rec aux (line : char list) idx =
        let chunk, rest = line |> List.splitAt 14
        if chunk |> List.distinct |> List.length = 14 then idx
        else aux (removeFirst line)  (idx + 1)
        
    aux chars 14

let result = 
    input
    |> Seq.map getPacketMarker
    |> Seq.min 

printfn "%i" result 


