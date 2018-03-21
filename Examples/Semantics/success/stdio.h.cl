class Stdio inherits IO {

    scanfInt() : Int {
        in_int()
    };

    scanfString() : String {
        in_string()
    };

    printInt(n : Int) : Int {
        out_int(n)
    };

    printString(s : String) : String {
        out_string(s)
    };
};

class Main inherits Stdio {

    a: Int <- 1;
    b: Int <- 2;
    s : String <- "hola";

    main(): String {
        {
            a <- scanfInt();
            s <- scanfString();
            
            printInt(a);
            printString(s);
            
            printInt(1+1*5);
            printString("Hello World\n");
        }
    };
};