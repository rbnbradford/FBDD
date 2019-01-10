module Bdd.MakeTest

open Bdd.Context
open Expecto
open Gherkin

let private makeTestCaseOfScenario context (scenario: Scenario) =
  testCase scenario.Name <| fun _ -> context |> execute (scenario.Steps |> List.ofSeq) context.InitialState

let private makeTestListOfFeature context (feature: Feature) =
  feature.Scenarios |> Seq.map (makeTestCaseOfScenario context) |> List.ofSeq |> testList (sprintf "%s/%s" feature.Name context.Name)

let executeFeatureFile context filePath = filePath |> parseFeatureFile |> makeTestListOfFeature context
