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
### Succinct

    let square x = x * x
    let xs = [1..5] |> List.filter (fun x -> x % 2) |> List.map square

    (*** include-value: xs ***)

    type Quantity = Quantity of int
    type Bacon = Uncooked | Chewy | Crispy
    type Order = (Bacon * Quantity) list

    let foodList = [(Uncooked,0);(Chewy,5);(Crispy,2)]

***

### What I use it for
 - Presenting at meetups
    - FsReveal, show markdown, code samples
'TCBay

***

### Syntax
 - Two keywords story, type/let
 - DU , ADT  

***

### Features/What it enables
 - DDD / DSLs
 - DU
 - Computation expressions
 - Immutable by default
' Pragmatic
' Do the boring work

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

    let a = 5
    let factorial x = [1..x] |> List.reduce (*)
    let c = factorial a

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

