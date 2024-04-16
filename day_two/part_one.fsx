#r "nuget: unquote"

open swensen.unquote
open system.io
open system.collections.generic
open system
open system.linq

// let input = 
//     [| 
//     "a y";
//     "b x";
//     "c z";
//     |] 
//     :> seq<string>

let path = "./input.txt"

let input = file.readalllines(path)

type Shape =
    | Rock
    | Scissors
    | Paper

type Result =
    | Win
    | Lose 
    | Draw

let findResult (opponentPlay, myPlay) =
    match (opponentPlay, myPlay) with
    | ( Rock, Rock ) -> Draw
    | ( Rock, Scissors ) -> Lose
    | ( Rock, Paper ) -> Win
    | ( Paper, Paper ) -> Draw
    | ( Paper, Rock ) -> Lose
    | ( Paper, Scissors ) -> Win
    | ( Scissors, Scissors ) -> Draw
    | ( Scissors, Paper ) -> Lose
    | ( Scissors, Rock ) -> Win

let scoreResult result =
    match result with
    | Lose -> 0
    | Draw -> 3
    | Win -> 6

let scoreShape shape =
    match shape with
    | Rock -> 1
    | Paper -> 2
    | Scissors -> 3

let opponentPlayToShape p =
    match p with
    | "A" -> Rock
    | "B" -> Paper
    | "C" -> Scissors
    | _ -> failwith $"unknown play {p}"
  
let myPlayToShape p =
    match p with
    | "X" -> Rock
    | "Y" -> Paper
    | "Z" -> Scissors
    | _ -> failwith $"unknown play {p}"

let scoreRound (round: string) =
    let plays = round.Split [|' '|]
    let opponentPlay = opponentPlayToShape plays[0]
    let myPlay = myPlayToShape plays[1]

    let r = ( 
        (opponentPlay), 
        (myPlay) 
    )

    (scoreResult (findResult r)) + (scoreShape myPlay)

let result = 
    input 
    |> Seq.map scoreRound
    |> Seq.sum

printfn "%i" result    
