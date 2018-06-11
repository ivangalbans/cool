class Hello {
   foo: Int <- 8 * 2;
   faa: Int <- 9*5+1;
   fii: Int <- 6*3;
   
   moo2(a:Int, b:Int, c:Int, d:Int) : Int {
      a * b * c * d
   };

};

class Main {
  main():Int {
    let
      t:Hello <- new Hello
    in {
      t.moo2(1,2,3,4);
    }
  };
};