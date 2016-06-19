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
  - Time to market
 - **Statically** , Strong, **Inferred**
' Static - types must match
' Strong - No strings to int, must be explicit
' Inferred - Compiler goes through code, checks how it's being used, then assigns type 
' Some dynamic, from interop with C#, not popular, has it's uses
 - ***Functional First***, Multiparadigm as needed
' Compared to Clojure - type system and things like Scott's DDD posts/slides could be beneficial
' https://www.quora.com/Is-F-F-Sharp-better-than-Scala-If-so-why?share=1
' http://techneilogy.blogspot.fr/2012/01/f-vs-scala-my-take-at-year-two.html
' Pragmatic
' Do the boring work
' Immutable by default
' No nulls
' Correctness
' Explicit

***

### .NET ecosystem
- Where does it fit in
' Ivory tower 
- Very active community 
 
---

### Open Source Projects/Community
- Active, small, tight community
- Upper eschilons community are language junkies
- FsReveal
- FParsec
- Neat TypeProviders
- Ionide
- FAKE (F# Make)/Paket (Dependency management)
' Steffen Forkmann - 5200 contributions last year
' Don Syme - 2650 contributions
' Krzysztof Cieślak - 1500 contributions
' Tomas Petricek - 1200 contributions
' Alfonso Garcia - 1000 contributions

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
' REPL   

---

    [lang=csharp]
    public class Program {
        public int square(int x)
        {
            return x * x;
        }
        public static List<int> Main()
        {
            List<int> ints = new List<int>();
            for (int i = 1; i <= 5; i++)
            {
                if (i % 2 == 0) 
                {
                    ints.Add(square(i));
                }
            }
            return ints;
        }
    }

---

    [lang=csharp]
    
    public class Program {
        public static List<int> Main()
        {
            var square = (x) => x * x;
            return xs = Enumerable.Range(1,5).
                        Where((x) => x % 2 == 0).
                        Select(square);
        }
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

---

Pipe operator |> 
- Connect the output to the input of the next
- Brackets means infix operator
*)

let (|>) x f = f(x) 
let a = 4 |> add5 |> printfn "%A"

(**
Pipe <|
- Same thing, but to the right side
*)
let (<|) g x = g(x)
let b = add5 <| (1 + 1)

(**---
*)
let thedarkside = 1 |>(+)<| 2

let λ = 1 + 2

let ``The output of this test shpuld = 💩`` = 2

(**
### Type Safety
- Functions will be as generic as possible, until they use something that binds them to a type
- Uses lamdba calculus to formulate what types are intended on being used
- Can't shove Bacon into something that wants Vegitables
- Re-working domain will lead to compile errors instead of runtime errors

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

let baconresult = 
    match Chewy with
    | Delicious s -> sprintf "Yay: %s" s
    | Eww s -> sprintf "Nooooo: %s" s
(** val result : *)
(*** include-value: baconresult ***)

(**

***
### Units of Measure
' let acceleration = 2.*(distance/time-initialVelocity)/time
' The unit of measure 'm/sec ^ 2' does not match the unit of measure 'm/sec'
*)

[<Measure>] type m = class end
[<Measure>] type s = class end
[<Measure>] type kg = class end
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
    [<Measure>] type C = class end
    [<Measure>] type F = class end

    let CtoF c = 
        c * 1.8<F/C> + 32.0<F>
    let Far =  CtoF 21.<C>
(*** include-value: Far ***)
(**

***

### Computation expressions

- Syntactic sugar for monadic binds
    - Looks like imperative as imperative
- Lead to LINQ, Async/Await in C#
- Similar in function to Haskell do, or Scala for {} yield
- https://fsharpforfunandprofit.com/series/computation-expressions.html
- By default we get seq {} and async {}
' These are lazy
- Implementation up to the builder. 

---

*)
let urls = seq {
        yield "http://google.com"
        yield "http://yahoo.com"
        yield! [for i in [0..5] -> "http://google.com?q=" + i.ToString()]
    } 
let list = urls |> Seq.toList
(*** include-value:list ***)
(**

---

*)
#r "System.Net.Http"
open System
open System.Net.Http
let longRunning url = 
    async {
        use client = new HttpClient()        
        let! result = Uri(url) |> client.GetAsync |> Async.AwaitTask
        return result
    }
let text = urls |> Seq.map longRunning  |> Async.Parallel


(**


---

    - return : a -> Special<a>
    - bind : (a -> Special<b>) -> Special<a> -> Special<b>
    - map : (a -> b) -> Special<a> -> Special<b>
    - apply : Special(a->b) -> Special<a> -> Special<b>

---

*)

let bind f xOpt = 
        match xOpt with
        | Some x -> f x
        | _ -> None

type MaybeBuilder() =
     member this.Bind(m, f) = bind f m
     member this.Return(x) = Some x
let maybe = MaybeBuilder()

(**

---

Turn 
*)

let optR one two three = 
    bind (fun x -> 
        bind (fun y -> 
            bind(fun z -> Some (x + y + z)) three) two) one

(**
Into 
*)
let optRe one two three =  maybe {
    let! x = one 
    let! y = two
    let! z = three
    return x + y + z  
}
(**---*)
(***define-value:opt***)
let opt = optRe (Some 4) (Some 2) (Some 1)
(***include-value:opt***)
let opt2 = optRe (Some 4) (Some 2) None
(**
    val it : None
    *)
(**

---

*)
let products = Map.empty<string,int> |> Map.add "Veggies" 1  |> Map.add "Bacon" 2
let customers = Map.empty<string,int> |> Map.add "Dan"  1 |> Map.add "Chris" 2
type Order = {Customer:int;Product:int}
(**---*)
let orderProduct customerKey productKey =
    match Map.tryFind customerKey customers with 
    | None -> None
    | Some cust -> match Map.tryFind productKey products with 
                | None -> None
                | Some prod -> Some {Customer=cust;Product=prod} 
                
let _ = match orderProduct "Dan" "Bacon" with 
        | Some prod -> printfn "%A" prod
        | None -> ()
(**
    Some 1 *)
let _ = match orderProduct "Chris" "Not veggies"  with 
        | Some prod -> printfn "%A" prod
        | None -> ()
(**
    None *)

(**

---

*)

let orderProduct2 customerKey productKey = maybe {
    let! cust = Map.tryFind customerKey customers 
    let! prod = Map.tryFind customerKey customers
    return {Customer=cust;Product=prod} 
}
maybe { let! result = orderProduct2 "Dan" "Bacon"
        return result.Product }
(**
    Some 1 *)
maybe { let! result = orderProduct2 "Chris" "Not veggies"
    return result.Customer }
(**
    None *)
(**

---

Short Demo?

' let u = 45 //int 
' let x = Some 45 //int option | Option<int>
' let y = None //a' option | Option<'a>, will stay generic until you use it
' 
' let a = u + x //Compiler error, how to combine int, int option
' let (Some x') = x
' let a = u + x'
' let (Some y') = y //Runtime error 
' let add v = 
'     match v with 
'     | Some v -> Some (u + v)
'     | None -> None 
'  

---

    let cluster = AzureCluster.Connect(config, 
                                    logger = ConsoleLogger(true), 
                                    logLevel = LogLevel.Info)
    let localResult = cloud { 
            printfn "hello, world" 
            return Environment.MachineName 
        } |> cluster.RunLocally

    let remoteResult = cloud { 
            printfn "hello, world" 
            return Environment.MachineName 
        } |> cluster.Run                          

***

### Domains used
####Everything
- F# Advent Calendar
 - (https://sergeytihon.wordpress.com/2014/11/24/f-advent-calendar-in-english-2014/)
 - (https://sergeytihon.wordpress.com/2015/10/25/f-advent-calendar-in-english-2015/)
 - http://fsharpworks.com/survey.html
 - Domain Modelling/Enterprise dev/Commercial software/Web
 - Data Science/Machine Learning
 - Where ever C#/VB are used
 - Fiddling/Tinkering
 - Microservices
' Financials
' Quake port
' Mapping of Star Wars characters to connectedness in the story
' GPU Processing
' Parsers
' Creating Business Domains
' Data Science

***

###What I use it for
- Learning Functional Programming
 - Start with what I know, then go closer to pure
 - Find some balance between FP and algorithms I know
 

***

### How to get started
#### Resources
- https://FSharp.org - Installs for Windows/Linux/OSX
 - For full Visual Studio, install FSharp Power Tools
 - For Visual Studio Code (lightweight free) or Atom, use Ionide. 
- https://fsprojects.github.io
- FSharp Weekly https://sergeytihon.wordpress.com/category/f-weekly/
- Community for F# https://C4FSharp.net
- FSharp For Fun and Profit https://fsharpforfunandprofit.com 
 - Domain driven design https://fsharpforfunandprofit.com/ddd/
 - 26 Ways to use F# at work https://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work/
- https://fpchat.com #fsharp-beginners #fsharp #<lang of choice>

***

### Demo 2

***

### What is FsReveal?

- Generates [reveal.js](http://lab.hakim.se/reveal-js/#/) presentation from [markdown](http://daringfireball.net/projects/markdown/)
- Utilizes [FSharp.Formatting](https://github.com/tpetricek/FSharp.Formatting) for markdown parsing
- Save / Refresh automatically ( build / F5 )
- Publish to github pages ( build ReleaseSlides )
- Get it from [http://fsprojects.github.io/FsReveal/](http://fsprojects.github.io/FsReveal/)

---

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

Goodbye

http://github.com/kellerd/Meetup-FSharp-Primer-Slides
http://github.com/kellerd/FsLabTutorial
http://github.com/kellerd/TicTacToeProvider

*)