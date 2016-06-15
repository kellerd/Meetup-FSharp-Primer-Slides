(**
- title : F# Primer
- description : Introduction to F#
- author : Dan Keller
- theme : night
- transition : default

***

### FSharp Primer

![F#](images/fsharp256.png)

  *A look at F# from a pragmatic an practical point of view*
 
---
### Language

 - Designed 2005, Released 2010 by Don Syme @ Microsoft Reasearch
 - Open Source, Since 2012
 - Managed by the FSharp Foundation, with engineering support from Microsoft
  - Expand Platforms, Promote, Train
 - Cross Platform - .NET (Soon Linux/Mac), Mono (Linux/Mac), *JavaScript*
  

---
### Design 
 - A Descendant of OCAML for the .NET Ecosystem
' v1.0 was almost a direct port, 2-3 awesome stuff, 4+ quality of life
' v1 Functional programming, DU, Pattern Matching, Records, Interop with .NET
' v2 Active Patterns, Units of Measure, Computation expressions
' v3 Type providers, More interop with .NET 4.0 features
' v4 Perf, Library functions, Yet another result type to be fixed
 - Very Succinct 
 - **Statically** , Strong, **Inferred**
' Static - types must match
' Strong - No strings to int, must be explicit
' Inferred - Compiler goes through code, checks how it's being used, then assigns type 
' Some dynamic, from interop with C#, not popular, has it's uses
 - ***Functional First***, Multiparadigm as needed
' Compared to Clojure - type system and things like Scott's DDD posts/slides could be beneficial
' https://www.quora.com/Is-F-F-Sharp-better-than-Scala-If-so-why?share=1
' http://techneilogy.blogspot.fr/2012/01/f-vs-scala-my-take-at-year-two.html

---

' Pragmatic
' Do the boring work
' Immutable by default

***

### More Succinct than most
*** well at least C# ***
*)
    let square x = x * x
    let xs = [1..5] 
             |> List.filter (fun x -> x % 2 = 0) 
             |> List.map square    

(** val xs : int list *)
(*** include-value: xs ***)
(**

---

    [lang=csharp]
    public static void Main()
    {
        var square = (x) => x * x;
        var xs = Enumerable.Range(1,5).
                    Where((x) => x % 2 == 0).
                    Select(square);
    }

***

### Partial application

*A function*

*)
let add x y = x + y
(**
*A partial application of that function*

*)
let add5 = add 5
(*** include-value: add5 ***)
(**
*Using that new function*
*)
let newResult = add5 6
(*** include-value: newResult ***)
(**

***

###Algebraic Data Types, Discriminated Unions

*)
type Bacon = Uncooked | Chewy | Crispy

type Tree<'Data> =
| Empty 
| Node of Tree<'Data> * 'Data * Tree<'Data> 

let baconTree = 
    Node(
        Node(Empty,Uncooked,Empty),
        Chewy,
        Node(Empty,Chewy,Node(Node(Empty,Chewy,Empty),
                                Crispy,
                                Node(Node(Empty,Chewy,Empty),
                                        Chewy, 
                                        Node(Empty,Uncooked,Empty)))))
(** 

---

### Small Basic value
*)

(*** include:dsl-1 ***)

(**
*[Phil Trelford Small Basic Parser](http://trelford.com/blog/post/interpreter.aspx)*

---

### Small Basic expression
*)

(*** include:dsl-2 ***)

(**

---

### Small Basic expression
*)

(*** include:dsl-3 ***)

(**

--- 

###FParsec

*)

(*** include:parsec-1 ***)
(**

--- 

*)

(*** include:parsec-2 ***)
(**

--- 

*)
(*** define: dsl-1 ***)
type value = // discriminated union
    | Bool of bool
    | Int of int
    | Double of double
    | String of string
type logical = Or | And | OrElse | AndAlso
type comparison = Eq | Neq | Gt | Lt | LtEq | GtEq
type identifier = string
type location = Index of int // Single discriminated union
type invoke = unit -> unit //Function type
type arithmetic = Add | Mul | Div | Sub
(*** define: dsl-2 ***)
type expr =
    | Literal of value
    | Var of identifier
    | GetAt of location
    | Func of invoke
    | Neg of expr
    | Arithmetic of expr * arithmetic * expr
    | Comparison of expr * comparison * expr
    | Logical of expr * logical * expr
(*** define: dsl-3 ***)
type label = string // Alias
type assign = Set of identifier * expr
type instruction =
    | Assign of assign
    | SetAt of location * expr
    | PropertySet of string * string * expr
    | Action of invoke
    | For of assign * expr * expr
    | EndFor
    | If of expr
    | ElseIf of expr
    | Else
    | EndIf
    | While of expr
    | EndWhile
    | Sub of identifier
    | EndSub
    | GoSub of identifier
    | Label of label
    | Goto of label
(*** define: parsec-1 ***)
#I @"../packages/FParsec/lib/net40-client"
#r "FParsec.dll"
#r "FParsecCS.dll"
open FParsec.CharParsers
open FParsec.Primitives
open System
let ptrue = pstring "true" |>> fun _ -> true
let pfalse = pstring "false" |>> fun _ -> false
let pbool = (ptrue <|> pfalse) |>> fun x -> Bool(x)
let pint = pint32 |>> fun n -> Int(n)
let pvalue = pbool <|> pint
let pliteral = pvalue |>> fun x -> Literal(x)
let result = run ptrue "true is correct"
// ...
//let parse = run pfor "For A=1 To 100"
//Success: For (Set ("A",Literal (Int 1)),Literal (Int 100),Literal (Int 1))
(*** define: parsec-2 ***)
let rec eval expr = 
    match expr with
        | Literal(value) -> value
     // | Var(identifier) -> ...
//             ...
        | Logical(expr1, logical, expr2) -> 
            match (eval expr1),logical,(eval expr2) with 
            | (Bool a), And, (Bool b) -> Bool(a && b) 
            | (Int a), And, (Int b) -> Bool(System.Convert.ToBoolean(a) && 
                                            System.Convert.ToBoolean(b)) 
            | _ -> failwith "Invalid combination"

(**

***

#### Active Patterns
*)
let (|Integer|_|) s =
  match System.Int32.TryParse(s) with
  | (true, n) -> Some n
  | _ -> None

let (|Float|_|) s =
  match System.Double.TryParse(s) with
  | (true, d) -> Some d
  | _ -> None

let checkInput s =
    match s with
    | Integer n -> sprintf "%d : int" n
    | Float d -> sprintf "%f : float" d
    | _ -> "Garbage"

(**

---

*)
#r "System.Drawing"
open System.Drawing
let (|RGB|) (col : Color) =
    ( col.R, col.G, col.B )

let (|HSB|) (col : Color) =
    ( col.GetHue(), col.GetSaturation(), col.GetBrightness() )

let checkColour c =
    match c with 
    | HSB (h,s,b) when b > 50.f -> sprintf "over half bright"
    | RGB (r,g,b)  -> sprintf "Some %d, %d, %d" r g b


(**

---

*)

let (|Delicious|Eww|) b = 
    match b with
    | Chewy | Crispy -> Delicious "Yummy"
    | _ -> Eww "Gross"

let result = 
    match Chewy with
    | Delicious s -> sprintf "Yay: %s" s
    | Eww s -> sprintf "Nooooo: %s" s
(** val result : *)
(*** include-value: result ***)

(**

***
### Units of Measure
' let acceleration = 2.*(distance/time-initialVelocity)/time
' The unit of measure 'm/sec ^ 2' does not match the unit of measure 'm/sec'
**)

[<Measure>] type m
[<Measure>] type s
[<Measure>] type kg
let distance = 135.0<m>    
let time = 4.0<s>    
let velocity = distance / time    
let initialVelocity = 0.7<m/s>   
let mass = 62.<kg>
let acceleration = 2.*(distance/time-initialVelocity)/time
let force = mass * acceleration
[<Measure>] type N = kg m/s^2
let forceDifference = force - 3.<N>
(*** include-value: forceDifference ***)
let momentum = mass * velocity
(*** include-value: momentum ***)
(**

---

*)
[<Measure>] type C
[<Measure>] type F

let CtoF c = 
    c * 1.8<F/C> + 32.0<F>
let Far =  CtoF 21.<C>

(*** include-value: CtoF ***)
(*** include-value: Far ***)

(**

***

### Computation expressions

***

### What I use it for
 - Presenting at meetups
    - FsReveal, show markdown, code samples
'TCBay

***

### Domains used

### .NET ecosystem
 - Where does it fit in
  - Ivory tower
 - Very active community 

' Steffen Forkmann - 5200 contributions last year
' Don Syme - 2650 contributions
' Krzysztof Cieślak - 1500 contributions
' Tomas Petricek - 1200 contributions
' Alfonso Garcia - 1000 contributions
***

### Open Source Projects/Community

***

### How to get started
### Resources
 - FSharp.org
 - C4FSharp
 - fsharpforfunandprofit

***

### Demo

***

### What is FsReveal?

- Generates [reveal.js](http://lab.hakim.se/reveal-js/#/) presentation from [markdown](http://daringfireball.net/projects/markdown/)
- Utilizes [FSharp.Formatting](https://github.com/tpetricek/FSharp.Formatting) for markdown parsing
- Get it from [http://fsprojects.github.io/FsReveal/](http://fsprojects.github.io/FsReveal/)

![FsReveal](images/logo.png)

***

### Reveal.js

- A framework for easily creating beautiful presentations using HTML.


> **Atwood's Law**: any application that can be written in JavaScript, will eventually be written in JavaScript.

***

### FSharp.Formatting

- F# tools for generating documentation (Markdown processor and F# code formatter).
- It parses markdown and F# script file and generates HTML or PDF.
- Code syntax highlighting support.
- It also evaluates your F# code and produce tooltips.

***

### Syntax Highlighting

#### F# (with tooltips)
*)
    let a = 5
    let factorial x = [1..x] |> List.reduce (*)
    let c = factorial a

(**

---

#### C#

    [lang=cs]
    using System;

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello, world!");
        }
    }

---

#### JavaScript

    [lang=js]
    function copyWithEvaluation(iElem, elem) {
        return function (obj) {
            var newObj = {};
            for (var p in obj) {
                var v = obj[p];
                if (typeof v === "function") {
                    v = v(iElem, elem);
                }
                newObj[p] = v;
            }
            if (!newObj.exactTiming) {
                newObj.delay += exports._libraryDelay;
            }
            return newObj;
        };
    }


---

#### Haskell
 
    [lang=haskell]
    recur_count k = 1 : 1 : 
        zipWith recurAdd (recur_count k) (tail (recur_count k))
            where recurAdd x y = k * x + y

    main = do
      argv <- getArgs
      inputFile <- openFile (head argv) ReadMode
      line <- hGetLine inputFile
      let [n,k] = map read (words line)
      printf "%d\n" ((recur_count k) !! (n-1))

*code from [NashFP/rosalind](https://github.com/NashFP/rosalind/blob/master/mark_wutka%2Bhaskell/FIB/fib_ziplist.hs)*

---
#### Scala
 
    [lang=scala]
    object Pi {
        class PiIterator extends Iterable[BigInt]{
            var r:BigInt=0
            var q, t, k:BigInt=1
            var n, l:BigInt=3
            var nr, nn:BigInt=0
            //...
        }
        
        def main(args: Array[String]): Unit = {
            val it=new PiIterator
            println((it head) + "." + (it take 300 mkString))
        }
    }
---

#### Clojure
 
    [lang=clojure]
    (defn ints-from [n]
    (cons n (lazy-seq (ints-from (inc n)))))
    
    (defn drop-nth [n seq] 
    (cond 
        (zero?    n) seq
        (empty? seq) []
        :else (concat (take (dec n) seq) (lazy-seq (drop-nth n (drop n seq))))))

---
#### Q
 
    [lang=q]
    w:400; h:300; r:150; l:-0.5 0.7 0.5
    sqrt0:{$[x>0;sqrt x;0]};
    z:{[x;y;r]sqrt0((r*r)-((x*x)+(y*y)))};
    is:{[x;y;r]
    z0:z[x;y;r];
    s:(x;y;z0)%r;
    $[z0>0;i:0.5*1+(+/)(s*l);i:0];
    i};
    fcn:{[xpx;ypx]
    x:xpx-w%2;
    y:ypx-h%2;
    z1:z[x;y;r];
    x2:x+190;
    z2:170-z[x2;y;r];
    $[(r*r)<((x*x)+(y*y));
        $[y>-50;i:3#0;i:200 100 50];
        $[z2>z1;i:3#is[x;y;r]*140;i:3#is[(-1*x2);(-1*y);r]*120]
    ];
    "i"$i};
    \l bmp.q
    fn:`:demo.bmp;
    writebmp[w;h;fcn;fn];
---

![Deathstar](http://rosettacode.org/mw/images/8/83/Qdstar.jpg)

---

### Bayes' Rule in LaTeX

$ \Pr(A|B)=\frac{\Pr(B|A)\Pr(A)}{\Pr(B|A)\Pr(A)+\Pr(B|\neg A)\Pr(\neg A)} $

***

### The Reality of a Developer's Life 

**When I show my boss that I've fixed a bug:**
  
![When I show my boss that I've fixed a bug](http://www.topito.com/wp-content/uploads/2013/01/code-07.gif)
  
**When your regular expression returns what you expect:**
  
![When your regular expression returns what you expect](http://www.topito.com/wp-content/uploads/2013/01/code-03.gif)
  
*from [The Reality of a Developer's Life - in GIFs, Of Course](http://server.dzone.com/articles/reality-developers-life-gifs)*



*)