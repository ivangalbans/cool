class Hello {
   foo: Int <- 8 * 2;
   faa: Int <- 19;
   
   moo2(a:Int, b:Int, c:Int, d:Int) : Int {
      a * b * c * d
   };

   moo1(a:Int, b:Int, c:Int, d:Int) : Int {
      a + b + c + d
   };

};

class Byte inherits Hello{
  --faa: Int <- 18;

  moo2(a:Int, b:Int, c:Int, d:Int) : Int {
      2 * a * b * c * d
   };
};

class Main {
  bbb: Hello;
  aaa: Int <- 3;
  ccc: String <- "HOLA MUNDO";

  bar():Object { while (not false) loop ("Ooga booga") pool};


	moo3(a:Int, b:Int, c:Int, d:Int) : Int {
	  a * b * c * d
	};

  main():Int {
    let
      t:Hello <- new Hello
    in {
      t.moo2(1,2,3,4);
      --moo3(1,2,3,4);
    }
  };
};