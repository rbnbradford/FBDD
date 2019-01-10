module Bdd.Context

type private StepArgs = string list
type private StepHandler<'testState> = 'testState -> StepArgs -> 'testState
type private AssertionStepHandler<'testState> = 'testState -> StepArgs -> Unit

type Context<'state> = {
    Name: string
    InitialState: 'state
    StepDefinitions: Map<string, StepHandler<'state>>
  }

let create name initialState =
  {
    Name = name
    InitialState = initialState
    StepDefinitions = Map.empty<string, StepHandler<_>>
  }

let private registerStep template handler context = { context with StepDefinitions = context.StepDefinitions.Add(template, handler) }
let Given = registerStep
let When = registerStep
let Then template (assertion: AssertionStepHandler<'a>) = registerStep template (fun state args -> assertion state args; state)

module private Step =
  let split chars (s: string) = s.Split chars
  let withoutFirstWord (s: string) = s |> split [| ' ' |] |> Array.skip 1 |> String.concat " "
  let decompose (s: string) =
    s
    |> withoutFirstWord
    |> split [| '{'; '}' |]
    |> Array.indexed
    |> Array.partition (fun (i, _) -> i % 2 = 0)
    |> fun (templateParts, parameters) ->
      (
        templateParts |> Array.map snd |> String.concat "{}",
        parameters |> Array.map snd |> List.ofArray
      )

let private runLine step state context =
  let definitions = context.StepDefinitions
  let template, parameters = Step.decompose step
  if definitions.ContainsKey template
  then definitions.[template] state parameters
  else failwithf "No step defined for template `%s`" template

let rec internal execute lines state context =
  match lines with
  | [] -> ()
  | line :: remainingLines ->
    let newState = runLine line state context
    execute remainingLines newState context
