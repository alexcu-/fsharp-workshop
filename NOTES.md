# F# Workshop Notes

## Introduction

- Statically typed
- Type interence engine
- Promotes immutability, can't mutate state
- Whitespace _is_ significant
- Single-pass compiled; starts from the start and ends at the end. You can't call things unless they are defined. Can only use what you have defined.

## Using the REPL (F# interactive)

If you want to evaluate something in the REPL, append a double semicolon at the end of the line:

```
> let b = 10;;
```

Otherwise, select your code in the editor and hit `^⏎` to evaluate it in the REPL

## Functions

```
let a = 1
```
This is **not a function**. This is just a _binding_ of 1 to the name _a_.

```
let increment a = a + 1
```

This **is a function**. Functions are _first-class_. The syntax is identical as the binding above. Increment is the name, and it takes an argument `a`. The result? Add 1 to a.

Now calling it:

```
> increment 10;;

val it : int = 11
```

Functions allow a lot of characters in their names, can even use whitespace using backticks

```
let ``increment my value`` a = a + 1
```

Pretty nice to for tests, as we can have punctuation.

### Types and type inference

Type signature of the function of `a` was inferred:

![](http://puu.sh/jE86v/af8d6dcdf6.png)

Takes an `int` and returns an `int`

```
int -> int
```

It's inferred here, but you can be explicit:

```
let increment' (a : int) : int = a + 1
```

![](http://puu.sh/jE8a5/079f6c7beb.png)

As you can see, they are the same signatures

Changing the increment to `1.0` would infer `a` as a `float` instead:

![](http://puu.sh/jE8cs/3e1ed48bb4.png)

You can also specify return types

```
let increment''' : int -> int = fun a -> a + 1
```

The `int -> int` means that you constrain the signature, and here we're constraining it using an anonymous function (below).

The `int -> int` belongs to the `increment'''` binding, not the anonymous function:

```
let increment''' : int -> int
```

#### Anonymous function

You can also have anonymous functions using the `fun` keyword

```
let increment'' = fun a -> a + 1
```

Same type signature, it's inferred from the anonymous function `fun a -> a + 1`:

![](http://puu.sh/jE8na/c99c5d649d.png)

### Currying and signatures with multiple parameters

Functions only take **one** argument. Add `a` is a argument that returns another argument. Remember, from LSD, this concept is known as **currying**.

![](http://puu.sh/jE8GG/c0268f3b9b.png)

Think of it like:

```
  a       b      ret
(int -> (int -> int))
            fn
      fn
```

This means we can **call a function with an incomplete set of arguments**:

```
> add 1;;

val it : (int -> int) = <fun:it@52-5>
```

Returns a partially complete function, still needs a new parameter. If we bind this partial function to a name, we can partially apply the function, say `inc` to `add 1`, then using `inc 3` to return `4`:

```
> let inc = add 1;;

val inc : (int -> int)

> inc 3;;

val it : int = 4
```

Hence, we can **pass functions around** partially.


When we're calling a function, we don't need to add the parens. But the evaluation is LTR; the evaluation to `f` takes precedence over the `+ 2`. The `(fun x -> x * 2)` is the `f` which we pass in `n` to.

```
let applyFunctionThenAdd2 f n = 
    f n + 2

Examples.test "Multiply by two then add two" (fun () ->
    applyFunctionThenAdd2 (fun x -> x * 2) 10 = 22
)
```

#### Pipelining 

Pipeline operator (`>|`) flips the function call around:

Thus: `x |> f = fx`:

```
let examplePipe n =
    n
    |> add 10      (* n is piped into here as add 10 n        *)
    |> increment   (* output of above is piped into increment *)
    |> add 20      (* output of increment is piped to add 20  *)
```

This is essentially:

```
(add 20 (increment (add 10 n)))
```

As you can see, we would evaluate `(add 10 n)`, then pass that result to `increment` then pass that result to `add 20`.

#### Custom Operators

We can define custom operators wrapped in parens:

```
// the pipe operator has a very simple implementation
// try implementing ||> to do the same things as pipe
let (||>) x f = f x
```

Here we just implemented the pipe operator

### Scoping

Inner functions hide their scope from outer functions

### Recursion

If you want to do easy recursion, by using `rec` keyword.

If the last thing in your recursive function calls the recursive function itself, then the compiler will optimise it. You **will not build up the stack** by doing this because F# will handle it.

![](http://puu.sh/jEa2u/98e82963ff.png)

I was missing the `rec` keyword before the `factorial` definition

### Conditional branching

When conditional branching, your return must be the same on all branches.

```
if predicate exp1 exp2 then
  "hello"
else
  "world"
```

is okay but

```
if predicate exp1 exp2 then
  1234
else
  "world"
```

isn't.

## Tuples

- Tuples are a comma separated collection of values.
- Useful for passing stuff around

The signature of tuples are inferred with `*`, not a `->` like functions:

```
val vect1 : float * float = (5.0, 14.5)
val fileSize : string * int = ("/var/log/err.log", 452264)
```

### Decomposing / pattern matching

We can break down tuples into different labels:

```
> let fileSize   = ("/var/log/err.log", 452264);;

val fileSize : string * int = ("/var/log/err.log", 452264)

> let file, size = fileSize;;

val size : int = 452264
val fileName : string = "/var/log/err.log"
```

Don't care about the size of that file? Use an `_` to ignore it when decomposing:

```
> let fileName', _ = fileSize;;

val fileName' : string = "/var/log/err.log"
```

Tuples used for C# `out` references (to avoid state change):

```
> let parsed = System.Int32.TryParse "1234";;

val parsed : bool * int = (true, 1234)

> let parsed = System.Int32.TryParse "banana";;

val parsed : bool * int = (false, 0)
```

Now we can use decomposing to pluck out the `true` and `1234` or `false` and `0` in either case:

```
> let didItParse, _ = parsed

val didItParse : bool = false
```

## Records

- Like tuples but with named fields
- Essentially, structs
- Records are composable; can have a record in a record
- Records are **immutable**

Use the keyword `type` to create a new Record type

```
type Coord2d = { x : float; y : float }
let vect1    = { x = 3.5  ; y = 5.6   }
```

We create **values** (not *instances*) of `Coord2d`. The type inference will match `vect1` with `Coord2d`. The inference engine will use the most recent definition of the Record, so if we duplicated `Coord2d` to `Vector` just underneath `Coord2d`, then `Vector` would be matched instead.

```
type Player = { name: string;    coordinates: Coord2d   }
let someone = { name = "Freddy"; coordinates = vect1    }
```

### Pseudo-mutability with `with` keyword

Remember, it's not mutable. So, we take an existing value and create a new one to, say, move `someone` somewhere else:

```
let someoneMoved = { someone with coordinates = { x = 0.0; y = 0.0 } }
```

We can use a new line instead of a semicolon:

```
let vect2 = { vect1 with 
                        x = 0.0
                        y = 0.0 }
```

If we want to reference out own value that we're 'mutating', we can do that too:

```
let someoneMoved = { someone with coordinates = { x = coordinates.x + 1; y = 0.0 } }
```

### Deconstructing Records

We can deconstruct records too, just like tuples:

```
let someonesCoordinates = someone.coordinates;
```

### Equality Checks

Records have **structural equality** semantic. Compared on the values. Unlike F# classes, which is **referenced equality**, compared instances on the references in memory.

```
type Coord2dClass(x : float, y : float) = class
    member m.X = x
    member m.Y = y
end

let v1 = new Coord2dClass(5.0, 5.0)
let v2 = new Coord2dClass(5.0, 5.0)

v1 = v2   (* val it : bool = false *)
```

vs

```
let v1 = { x = 5.0; y = 5.0 }
let v2 = { x = 5.0; y = 5.0 }

v1 = v2   (* val it : bool = true *)
```

We only really use classes for C# interoperability—you don't *usually* use classes.