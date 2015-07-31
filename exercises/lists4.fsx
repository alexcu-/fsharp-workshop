(**********************************************************************************************************************
    In this section we'll go through implementing some of the common list functions that you would have come across
    in the previous exercises.
*)

#load "./examples.fs"
open Examples


(**********************************************************************************************************************
    They can also contain functions and be fairly complex. Use F# Interactive to see the result of the list
    comprehensions below
*)

let fib25 = [
    let rec loop i a b = [
      if i > 0 then
        yield b
        yield! loop (i-1) b (a+b)
    ]

    yield! [ 0; 1 ]
    yield! loop 23 1 1
  ]

let prime100 = [
    let isprime n =
      let rec check i =
        i > n/2 || (n % i <> 0 && check (i + 1))
      check 2

    for n in 2 .. 100 do
      if isprime n then
        yield n
  ]


(**********************************************************************************************************************
    Write a list comprehension that returns pairs of the first twenty five numbers from the Fibonacci sequence

    eg. (0, 1), (1, 1), (1, 2), (2, 3), ...
*)

// fibPairs: () -> (int * int) list
let fibPairs () = failwith "todo"

test "Create a list comprehension that returns pairs of the first 25 Fibonacci numbers" (fun () ->
  fibPairs () = [(0, 1); (1, 1); (1, 2); (2, 3); (3, 5); (5, 8); (8, 13); (13, 21); (21, 34);
    (34, 55); (55, 89); (89, 144); (144, 233); (233, 377); (377, 610);
    (610, 987); (987, 1597); (1597, 2584); (2584, 4181); (4181, 6765);
    (6765, 10946); (10946, 17711); (17711, 28657); (28657, 46368)]
)


(**********************************************************************************************************************
    Write a list comprehension that outputs the first 20 triangular numbers. 
    A triangular number is the number of objects that are need to make up an equilateral triangle, with the length of
    the sides being the iteration.

    eg.
                                                             *
                                          *                 * *
                          *              * *               * * *
    1 (1):  *    2 (3):  * *    3 (6):  * * *    4 (10):  * * * *
*)

// triangle10: () -> int list
let triangle10 () = failwith "todo"

test "Create a list comprehension that calculates the first ten triangular numbers" (fun () ->
  triangle10 () = [1; 3; 6; 10; 15; 21; 28; 36; 45; 55]
)




(**********************************************************************************************************************
    The following example shows walking through the list to perform an action on each item
*)

// walkFib: () -> ()
let walkFib () =
  let rec loop action =
    function
    | []      ->
      action None
    | x :: xs ->
      action (Some x)
      loop action xs
  loop (function | Some x -> printfn "%d" x | _ -> printfn "End of list") fib25


(**********************************************************************************************************************
    Using the above few functions as inspiration see if you can write a function to return the last item in a list.
*)

// last: 'a list -> a
let last list = failwith "todo"


test "Write a function to return the last item in a list" (fun () ->
  last fib25 = 46368

  &&

  last prime100 = 97
)

(**********************************************************************************************************************
    Mapping items within lists from one form to another is a common task. Write a generic list map function that
    takes as input a function to be applied to each item in the list and yields the result.

    It's acceptable to return unit () when yielding items if you do not want to yield a value at that point.
*)

// map: ('a -> 'b) -> 'a list -> 'b list
let map action list = failwith "todo"

test "Create a list map function" (fun () ->
  map (fun x -> x * 2) fib25 = [0; 2; 2; 4; 6; 10; 16; 26; 42; 68; 110; 178; 288; 466; 754; 1220; 1974;
    3194; 5168; 8362; 13530; 21892; 35422; 57314; 92736]

  &&

  map (fun x -> x * x) prime100 = [4; 9; 25; 49; 121; 169; 289; 361; 529; 841; 961; 1369; 1681; 1849; 2209;
    2809; 3481; 3721; 4489; 5041; 5329; 6241; 6889; 7921; 9409]
)


(**********************************************************************************************************************
    This time instead of mapping items in a list from one value to the next, write a filter function that takes a
    predicate and returns portion of a list based on that.
*)

// filter: ('a -> bool) -> 'a list -> 'a list
let filter predicate list = failwith "todo"

test "Write a function that filters a list using the given predicate" (fun () ->
  filter (fun x -> x % 2 = 0) fib25 = [0; 2; 8; 34; 144; 610; 2584; 10946; 46368]

  &&

  filter (fun x -> x % 10 = 3) prime100 = [3; 13; 23; 43; 53; 73; 83]
)


(**********************************************************************************************************************
    Write a function that uses match expressions to returns pairs of items
*)

// pairwise: ('a list) -> ('a * 'a) list
let pairwise list = failwith "todo"


test "Create a function that uses a match expression to return pairs of items from a list" (fun () ->
  pairwise fib25 = [(0, 1); (1, 1); (1, 2); (2, 3); (3, 5); (5, 8); (8, 13); (13, 21); (21, 34);
    (34, 55); (55, 89); (89, 144); (144, 233); (233, 377); (377, 610);
    (610, 987); (987, 1597); (1597, 2584); (2584, 4181); (4181, 6765);
    (6765, 10946); (10946, 17711); (17711, 28657); (28657, 46368)]

  &&

  pairwise prime100 = [(2, 3); (3, 5); (5, 7); (7, 11); (11, 13); (13, 17); (17, 19); (19, 23);
    (23, 29); (29, 31); (31, 37); (37, 41); (41, 43); (43, 47); (47, 53);
    (53, 59); (59, 61); (61, 67); (67, 71); (71, 73); (73, 79); (79, 83);
    (83, 89); (89, 97)]
)

(**********************************************************************************************************************
    Write a function can zip two lists of the same length together.

    eg. zip [ 1; 2; 3 ] [ 4; 5; 6] = [ (1, 4); (2, 5); (3, 6) ]
*)

// zip: a' list -> 'b list -> (a' list
let zip list1 list2 = failwith "todo"

test "Write a function can zip two lists of the same length together" (fun () ->
  zip fib25 prime100 = [(0, 2); (1, 3); (1, 5); (2, 7); (3, 11); (5, 13); (8, 17); (13, 19);
    (21, 23); (34, 29); (55, 31); (89, 37); (144, 41); (233, 43); (377, 47);
    (610, 53); (987, 59); (1597, 61); (2584, 67); (4181, 71); (6765, 73);
    (10946, 79); (17711, 83); (28657, 89); (46368, 97)]
)


(**********************************************************************************************************************
    Write a function that can sum the integers in a list.
*)

// sum: int list -> int
let sum list = failwith "todo"

test "Write a function that can sum the integers in a list" (fun () ->
  sum fib25 = 121392

  &&

  sum prime100 = 1060
)

(**********************************************************************************************************************
    You can make the function numerically generic by declaring it with 'inline' and using a special function
    for a generic zero:

    let inline sum list =
      ...
      loop (LanguagePrimitives.GenericZero<'a>) list
*)

// sum: 'a list -> 'a
let inline sum2 list = failwith "todo"


test "Write a function that can sum numeric values in a list" (fun () ->
  let floats = [0.02; 0.03; 0.05; 0.07; 0.11; 0.13; 0.17; 0.19; 0.23; 0.29; 0.31; 0.37;
    0.41; 0.43; 0.47; 0.53; 0.59; 0.61; 0.67; 0.71; 0.73; 0.79; 0.83; 0.89;
    0.97]

  let decimals = [0.02M; 0.03M; 0.05M; 0.07M; 0.11M; 0.13M; 0.17M; 0.19M; 0.23M; 0.29M; 0.31M;
    0.37M; 0.41M; 0.43M; 0.47M; 0.53M; 0.59M; 0.61M; 0.67M; 0.71M; 0.73M; 0.79M;
    0.83M; 0.89M; 0.97M]

  decimal (sum2 floats) = 10.6M

  &&

  sum2 decimals = 10.6M
)

(**********************************************************************************************************************
    Lets extend the previous function into something more generic. This time it needs to take a function (the folder)
    that performs the action on the item and accumulated value (replacing the '+' in in sum function. Next argument
    is state, effecitively the initializing value (like the zero in sum), and finally the list of values.
*)

// fold: ('state -> 'a -> 'state) -> 'state -> 'a list -> 'state
let fold (folder:'state->'a->'state) state list = failwith "todo"


test "Write a function to fold a list into a single value" (fun () ->
  fold (+) 0 fib25 = 121392

  &&

  fold (-) 0 prime100 = -1060

  &&
 
  fold (fun acc x -> acc + (sprintf "%d " x)) "Fib: " fib25 =
    "Fib: 0 1 1 2 3 5 8 13 21 34 55 89 144 233 377 610 987 1597 2584 4181 6765 10946 17711 28657 46368 "
)

(**********************************************************************************************************************
    BONUS: Try rewriting both the integer version and generic version of sum using fold.
*)

// sum: int list -> int
let sum3 = failwith "todo"

// sum: 'a list -> 'a
let inline sum4 list = failwith "todo"

test "Write a function that can sum the integers in a list using fold" (fun () ->
  sum3 fib25 = 121392

  &&

  sum3 prime100 = 1060
)


test "Write a function that can sum numeric values in a list using fold" (fun () ->
  let floats = [0.02; 0.03; 0.05; 0.07; 0.11; 0.13; 0.17; 0.19; 0.23; 0.29; 0.31; 0.37;
    0.41; 0.43; 0.47; 0.53; 0.59; 0.61; 0.67; 0.71; 0.73; 0.79; 0.83; 0.89;
    0.97]

  let decimals = [0.02M; 0.03M; 0.05M; 0.07M; 0.11M; 0.13M; 0.17M; 0.19M; 0.23M; 0.29M; 0.31M;
    0.37M; 0.41M; 0.43M; 0.47M; 0.53M; 0.59M; 0.61M; 0.67M; 0.71M; 0.73M; 0.79M;
    0.83M; 0.89M; 0.97M]

  decimal (sum4 floats) = 10.6M

  &&

  sum4 decimals = 10.6M
)


(**********************************************************************************************************************
    Now lets rethink fold in a different way, this time to reduce a list down to a single value using the supplied
    reducer function, but unlike fold, there's no initial state passed in as it starts with the first two items
    in the list first.

    Use the fold function in your solution.
*)

// reduce: ('a -> 'a -> 'a) -> 'a list -> 'a
let reduce reducer = failwith "todo"

 
test "Reduce a list down to a single value using the supplied function" (fun () ->
  reduce (*) prime100 = 833294374

  &&

  reduce (-) prime100 = -1056

  &&

  reduce (fun acc _ -> acc + 1) fib25 = 24

  &&

  reduce (+) ["a";"b";"c"] = "abc"
)

(**********************************************************************************************************************
    BONUS: Can you rewrite 'last' using reduce?
*)

let last2 list = failwith "todo"

test "Write a function to return the last item of list using reduce" (fun () ->
  last2 fib25 = 46368

  &&

  last2 prime100 = 97
)



(*
    References:
        Triangular number       - https://en.wikipedia.org/wiki/Triangular_number
        Inline functions        - https://msdn.microsoft.com/en-us/library/Dd548047.aspx
        GenericZero declaration - https://github.com/fsharp/fsharp/blob/master/src/fsharp/FSharp.Core/prim-types.fs#L2398
*)