module Bdd.Gherkin

type Scenario =
  {
    Name: string
    Steps: string seq
  }

type Feature =
  {
    Name: string
    Scenarios: Scenario seq
  }

let private parser = Gherkin.Parser()

let private convertFeature (feature: Gherkin.Ast.Feature) =
  let convertScenario (scenario: Gherkin.Ast.ScenarioDefinition) =
    {
      Name = scenario.Name;
      Steps = scenario.Steps |> Seq.map (fun step -> step.Keyword + step.Text)
    }
  {
    Name = feature.Name
    Scenarios = feature.Children |> Seq.map convertScenario
  }

let parseFeatureFile (filename: string) = parser.Parse(filename).Feature |> convertFeature
