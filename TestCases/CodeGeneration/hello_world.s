.data
buffer: .space 65536
strsubstrexception: .asciiz "Substring index exception
"
str0: .asciiz "IO"
str1: .asciiz "Object"
str2: .asciiz "Int"
str3: .asciiz "Bool"
str4: .asciiz "String"
str5: .asciiz "Main"
str6: .asciiz "Hello, World.\n"
_class.IO: .word str1, 0
_class.Int: .word str1, 0
_class.Bool: .word str1, 0
_class.String: .word str1, 0
_class.Main: .word str0, str1, 0

.globl main
.text
Object.type_name:
lw $a0, 0($sp)
lw $v0, 0($a0)
jr $ra

Object.copy:
Object.abort:
li $v0, 10
syscall

IO.out_string:
li $v0, 4
lw $a0, -4($sp)
syscall
jr $ra

IO.out_int:
li $v0, 1
lw $a0, -4($sp)
syscall
jr $ra

IO.in_string:
move $a3, $ra
la $a0, buffer
li $a1, 65536
li $v0, 8
syscall
addiu $sp, $sp, -4
sw $a0, 0($sp)
jal String.length
addiu $sp, $sp, 4
move $a2, $v0
addiu $a2, $a2, -1
move $a0, $v0
li $v0, 9
syscall
move $v1, $v0
la $a0, buffer
_io.in_string.loop:
beqz $a2, _io.in_string.end
lb $a1, 0($a0)
sb $a1, 0($v1)
addiu $a0, $a0, 1
addiu $v1, $v1, 1
addiu $a2, $a2, -1
j _io.in_string.loop
_io.in_string.end:
sb $zero, 0($v1)
move $ra, $a3
jr $ra

IO.in_int:
li $v0, 5
syscall
jr $ra

String.length:
lw $a0, 0($sp)
_str.length.loop:
lb $a1, 0($a0)
beqz $a1, _str.length.end
addiu $a0, $a0, 1
j _str.length.loop
_str.length.end:
lw $a1, 0($sp)
subu $v0, $a0, $a1
jr $ra

String.concat:
move $a2, $ra
jal String.length
move $v1, $v0
addiu $sp, $sp, -4
jal String.length
addiu $sp, $sp, 4
add $v1, $v1, $v0
addi $v1, $v1, 1
li $v0, 9
move $a0, $v1
syscall
move $v1, $v0
lw $a0, 0($sp)
_str.concat.loop1:
lb $a1, 0($a0)
beqz $a1, _str.concat.end1
sb $a1, 0($v1)
addiu $a0, $a0, 1
addiu $v1, $v1, 1
j _str.concat.loop1
_str.concat.end1:
lw $a0, -4($sp)
_str.concat.loop2:
lb $a1, 0($a0)
beqz $a1, _str.concat.end2
sb $a1, 0($v1)
addiu $a0, $a0, 1
addiu $v1, $v1, 1
j _str.concat.loop2
_str.concat.end2:
sb $zero, 0($v1)
move $ra, $a2
jr $ra

String.substr:
lw $a0, -8($sp)
addiu $a0, $a0, 1
li $v0, 9
syscall
move $v1, $v0
lw $a0, 0($sp)
lw $a1, -4($sp)
add $a0, $a0, $a1
lw $a2, -8($sp)
_str.substr.loop:
beqz $a2, _str.substr.end
lb $a1, 0($a0)
beqz $a1, _substrexception
sb $a1, 0($v1)
addiu $a0, $a0, 1
addiu $v1, $v1, 1
addiu $a2, $a2, -1
j _str.substr.loop
_str.substr.end:
sb $zero, 0($v1)
jr $ra

_substrexception:
la $a0, strsubstrexception
li $v0, 4
syscall
li $v0, 10
syscall

_stringcmp:
li $v0, 1
_stringcmp.loop:
lb $a2, 0($a0)
lb $a3, 0($a1)
beqz $a2, _stringcmp.end
beq $a2, $zero, _stringcmp.end
beq $a3, $zero, _stringcmp.end
bne $a2, $a3, _stringcmp.differents
addiu $a0, $a0, 1
addiu $a1, $a1, 1
j _stringcmp.loop
_stringcmp.end:
beq $a2, $a3, _stringcmp.equals
_stringcmp.differents:
li $v0, 0
jr $ra
_stringcmp.equals:
li $v0, 1
jr $ra


main:
# Call start;
sw $ra, 0($sp)
addiu $sp, $sp, -4
jal start
addiu $sp, $sp, 4
lw $ra, 0($sp)
# Object.constructor:


Object.constructor:
# PARAM t0;
# // set method: Object.abort
# *(t0 + 3) = Label "Object.abort"
la $a0, Object.abort
lw $a1, 0($sp)
sw $a0, 12($a1)
# // set method: Object.type_name
# *(t0 + 4) = Label "Object.type_name"
la $a0, Object.type_name
lw $a1, 0($sp)
sw $a0, 16($a1)
# // set method: Object.copy
# *(t0 + 5) = Label "Object.copy"
la $a0, Object.copy
lw $a1, 0($sp)
sw $a0, 20($a1)
# Return ;

lw $v0, 4($sp)
jr $ra
# IO.constructor:


IO.constructor:
# PARAM t0;
# PushParam t0;
lw $a0, 0($sp)
sw $a0, -12($sp)
# Call Object.constructor;
sw $ra, -8($sp)
addiu $sp, $sp, -12
jal Object.constructor
addiu $sp, $sp, 12
lw $ra, -8($sp)
# PopParam 1;
# // set method: IO.out_string
# *(t0 + 6) = Label "IO.out_string"
la $a0, IO.out_string
lw $a1, 0($sp)
sw $a0, 24($a1)
# // set method: IO.out_int
# *(t0 + 7) = Label "IO.out_int"
la $a0, IO.out_int
lw $a1, 0($sp)
sw $a0, 28($a1)
# // set method: IO.in_string
# *(t0 + 8) = Label "IO.in_string"
la $a0, IO.in_string
lw $a1, 0($sp)
sw $a0, 32($a1)
# // set method: IO.in_int
# *(t0 + 9) = Label "IO.in_int"
la $a0, IO.in_int
lw $a1, 0($sp)
sw $a0, 36($a1)
# Return ;

lw $v0, 4($sp)
jr $ra
# _class.IO: _class.Object
# _class.Int: _class.Object
# _class.Bool: _class.Object
# _class.String: _class.Object
# _wrapper.Int:


_wrapper.Int:
# PARAM t0;
# t1 = Alloc 7;
# Begin Allocate
li $v0, 9
li $a0, 28
syscall
sw $v0, -4($sp)
# End Allocate
# *(t1 + 0) = "Int"
la $a0, str2
lw $a1, -1($sp)
sw $a0, 0($a1)
# *(t1 + 6) = t0
lw $a0, 0($sp)
lw $a1, -4($sp)
sw $a0, 24($a1)
# Return t0;

lw $v0, 0($sp)
jr $ra
# _wrapper.Bool:


_wrapper.Bool:
# PARAM t0;
# t1 = Alloc 7;
# Begin Allocate
li $v0, 9
li $a0, 28
syscall
sw $v0, -4($sp)
# End Allocate
# *(t1 + 0) = "Bool"
la $a0, str3
lw $a1, -1($sp)
sw $a0, 0($a1)
# *(t1 + 6) = t0
lw $a0, 0($sp)
lw $a1, -4($sp)
sw $a0, 24($a1)
# Return t0;

lw $v0, 0($sp)
jr $ra
# _wrapper.String:


_wrapper.String:
# PARAM t0;
# t1 = Alloc 10;
# Begin Allocate
li $v0, 9
li $a0, 40
syscall
sw $v0, -4($sp)
# End Allocate
# *(t1 + 0) = "String"
la $a0, str4
lw $a1, -1($sp)
sw $a0, 0($a1)
# *(t1 + 9) = t0
lw $a0, 0($sp)
lw $a1, -4($sp)
sw $a0, 36($a1)
# Return t0;

lw $v0, 0($sp)
jr $ra
# _class.Main: _class.IO
# Main.main:


Main.main:
# PARAM t0;
# t1 = t0
lw $a0, 0($sp)
sw $a0, -4($sp)
# t3 = "Hello, World.\n"
la $a0, str6
sw $a0, -12($sp)
# // get method: Main.out_string
# t2 = *(t1 + 6)
lw $a0, -4($sp)
lw $a1, 24($a0)
sw $a1, -8($sp)
# PushParam t1;
lw $a0, -4($sp)
sw $a0, -20($sp)
# PushParam t3;
lw $a0, -12($sp)
sw $a0, -24($sp)
# t1 = Call t2;
sw $ra, -16($sp)
lw $a0, -8($sp)
addiu $sp, $sp, -20
jalr $ra, $a0
addiu $sp, $sp, 20
lw $ra, -16($sp)
sw $v0, -4($sp)
# PopParam 2;
# Return t1;

lw $v0, -4($sp)
jr $ra
# Main.constructor:


Main.constructor:
# PARAM t0;
# PushParam t0;
lw $a0, 0($sp)
sw $a0, -8($sp)
# Call IO.constructor;
sw $ra, -4($sp)
addiu $sp, $sp, -8
jal IO.constructor
addiu $sp, $sp, 8
lw $ra, -4($sp)
# PopParam 1;
# // set method: Main.main
# *(t0 + 10) = Label "Main.main"
la $a0, Main.main
lw $a1, 0($sp)
sw $a0, 40($a1)
# // set class name: Main
# *(t0 + 0) = "Main"
la $a0, str5
lw $a1, 0($sp)
sw $a0, 0($a1)
# // set class size: 11 words
# *(t0 + 1) = 11
lw $a0, 0($sp)
li $a1, 11
sw $a1, -4($a0)
# // set class generation label
# *(t0 + 2) = Label "_class.Main"
la $a0, _class.Main
lw $a1, 0($sp)
sw $a0, 8($a1)
# Return ;

lw $v0, 4($sp)
jr $ra
# start:


start:
# t1 = Alloc 11;
# Begin Allocate
li $v0, 9
li $a0, 44
syscall
sw $v0, -4($sp)
# End Allocate
# PushParam t1;
lw $a0, -4($sp)
sw $a0, -12($sp)
# Call Main.constructor;
sw $ra, -8($sp)
addiu $sp, $sp, -12
jal Main.constructor
addiu $sp, $sp, 12
lw $ra, -8($sp)
# PopParam 1;
# PushParam t1;
lw $a0, -4($sp)
sw $a0, -12($sp)
# Call Main.main;
sw $ra, -8($sp)
addiu $sp, $sp, -12
jal Main.main
addiu $sp, $sp, 12
lw $ra, -8($sp)
# PopParam 1;
li $v0, 10
syscall
