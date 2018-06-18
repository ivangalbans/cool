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
    let i:Int <- 1 in
      while i < 16 loop {
        out_int(i);
        out_string(" -> ");
        out_int(fibo(i));
        out_string("\n");
        i <- i+1;
      } pool
  };
};