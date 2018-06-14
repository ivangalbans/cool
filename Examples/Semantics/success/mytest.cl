class Hello {
   foo: Int <- 8 * 2;
   
   moo2(a:Int, b:Int, c:Int, d:Int) : Int {
      a * b * c * d
   };

};

class Main {
  bbb: Hello;

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