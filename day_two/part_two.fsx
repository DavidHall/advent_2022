#r "nuget: Unquote"

open Swensen.Unquote
open System.IO
open System.Collections.Generic
open System
open System.Linq

// let input = 
//     [| 
//     "A Y";
//     "B X";
//     "C Z";
//     |] 
//     :> seq<string>

let path = "./input.txt"

let input = File.ReadAllLines(path)


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

let instructionToResult i =
    match i with
    | "X" -> Lose
    | "Y" -> Draw
    | "Z" -> Win
    | _ -> failwith $"unknown instruction {i}"

let findPlay (opponentPlay, desiredResult) =
    match (opponentPlay, desiredResult) with
    | (_, Draw) -> opponentPlay 
    | (Rock, Win) -> Paper
    | (Paper, Win) -> Scissors
    | (Scissors, Win) -> Rock
    | (Rock, Lose) -> Scissors
    | (Paper, Lose) -> Rock
    | (Scissors, Lose) -> Paper

let scoreRound (round: string) =
    let plays = round.Split [|' '|]
    let opponentPlay = opponentPlayToShape plays[0]
    let desiredResult = instructionToResult plays[1]
    let myPlay = findPlay (opponentPlay, desiredResult)

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
