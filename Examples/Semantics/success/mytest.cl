class Main inherits IO {
  fibo(i:Int):Int {
    if i=0 then
      0
    else if i=1 then
      1
    else
      fibo(i-1)+fibo(i-2)
    fi fi
  };
    
  main():Object {
    if isvoid 0 then out_string("HI\n") else out_string("NOT HI\n") fi
  };
};