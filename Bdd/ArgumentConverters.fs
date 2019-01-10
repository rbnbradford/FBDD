module Bdd.ArgumentConverters

let (|IntArg|) s = 
  try System.Int32.Parse s
  with _ -> failwithf "'%s' is not a valid value for an integer argument" s

let (|BoolArg|) = fun s -> 
  match s with 
  | "true" | "True" -> true
  | "false" | "False" -> false
  | _ -> failwithf "'%s' is not a valid value for a boolean argument, provide 'true' or 'false'" s

let (|StringArg|) = id
