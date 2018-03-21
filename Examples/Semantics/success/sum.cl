class Pepe inherits IO {
    c: Int <- 5;

    consolewriteline(n : Int) : Int {
        out_int(n)
    };

};

class Main inherits Pepe {

    a: Int <- 1;
    b: Int <- 2;

    main(): Int {
        consolewriteline(a)
    };
};