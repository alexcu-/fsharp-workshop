(* 
 Tuples are a comma separated collection of values. 
 Each tuple can be considered as a set of all values of the types defined in the tuple.
 *)
#load "./examples.fs"


// Examples:

//
// Using a tuple to express some 2D coordinates or vectors.
//
let vect1 = (5.0, 14.5)
let vect2 = (20.7, 6.3)

//
// Tuples are not just restricted to two values.
// We could also express a 3D coordinate.
//
let box = (5, 14, 10) 


//
// Tuples can also contain mixed types
//
let fileSize = ("/var/log/err.log", 452264)

//
// Exercise 1: Tuple types
//   Evaluate the code above in the REPL.
//   (Hover the mouse over the vect1, vect2 & box to see the type signatures)
//
// You should see the following type signatures
//
// vect1 & vect2 : float * float
//           box : int * int * int
//      fileSize : string * int
//

//
// And important concept related to tuples is pattern matching 
// sometimes also know as destructuring.
// Pattern matching is covered in much more depth later on in
// the workshop.
//

//
// Exercise 2: Basic tuple pattern matching/deconstructing.
// 
// Execute the following line in the REPL.
let x, y = vect1

//
// Now deconstruct the box and fileSize tuples.
//

//
// Optional Exercise: Ignoring values
//
// Deconstrcut the fileSize tuple to get the file name and ignore the length
//


//
// Exercise 3:
//   Write a function to add two vectors.
//   (hint: watch the types)
//
let addVect (x1 : float, y1 : float) (x2 : float, y2 : float) = (x1 + x2, y1 + y2) // ** Answer **

//
// Exercise 4: Look at the type signature of System.Int32.TryParse.
//   Use the REPL or hover over it in the IDE text editor.
//
