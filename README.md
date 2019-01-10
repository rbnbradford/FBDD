## a.test =
```
module BddTests
#nowarn "25"

open Expecto
open Context
open Bdd
open ArgumentConverters 

type TestState = 
  {
    AgeRestriction: int
    Age: int
  }

let initialState = {AgeRestriction = 0; Age = 0}

let context =
  Context.create "context1" initialState
  |> Given "I am {}"
    (fun state [IntArg age] -> {state with Age = age})
  |> When "I must be over {}" 
    (fun state [IntArg age] -> {state with AgeRestriction = age})
  |> Then "I should be allowed" 
    (fun state _ -> Expect.isGreaterThanOrEqual state.Age state.AgeRestriction "should be allowed")
  |> Then "I shouldn't be allowed" 
    (fun state _ -> Expect.isLessThanOrEqual state.Age state.AgeRestriction "shouldn't be allowed")

[<Tests>]
let test = "bddFeatures/a.feature" |> executeFeatureFile context
```
## a.feature =
```
@feature
Feature: I can check ages

  Scenario: I am old enough
    Given I am {10}
    When I must be over {9}
    Then I should be allowed

  Scenario: I am not old enough
    Given I am {10}
    When I must be over {11}
    Then I shouldn't be allowed
```