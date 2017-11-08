using System;
using System.Collections.Generic;

namespace ConsoleApplication1 {
    class reflector{
        public string alf;
        public int i = 0;
        public reflector(string alf) {
            this.alf = alf;
        }
    }

    class rotor {
        public char c;
        public string alf;
        public int i = 0;
        public rotor(string alf, char c) {
            this.alf = alf;
            this.c = c;
        }
    }

    class enigma {
        private static rotor[] ROTORS = new rotor[6] {
            new rotor("abcdefghijklmnopqrstuvwxyz", 'a'), // 0, alphabet
            new rotor("ekmflgdqvzntowyhxuspaibrcj", 'r'), // 1
            new rotor("ajdksiruxblhwtmcqgznpyfvoe", 'f'), // 2
            new rotor("bdfhjlcprtxvznyeiwgakmusqo", 'w'), // 3
            new rotor("esovpzjayquirhxlnftgkdcmwb", 'k'), // 4
            new rotor("vzbrgityupsdnhlxawmjqofeck", 'a')  // 5
        };

        private static reflector[] REFLECTORS = new reflector[2] {
            new reflector("ejmzalyxvbwfcrquontspikhgd"),
            new reflector("yruhqsldpxngokmiebfzcwvjat")
        };

        //private List<rotor> rotors = new List<rotor> ();
        private rotor[] rotors;
        private reflector reflec;

        public enigma(int[] a, int revlector) {
            this.rotors = new rotor[a.Length];
            for(int i=0; i < a.Length; i++) this.rotors[i] = ROTORS[a[i]];
            this.reflec = REFLECTORS[revlector];
        }
        public static int ord(char c){
            return c - 97;
        }

        public static char chr(int c){
            return Convert.ToChar(c + 97);
        }

        public int getRotor(int i) {
            for (int j = 0; j < ROTORS.Length; j++)
                    if(this.rotors[i] == ROTORS[j]) 
                        return j;
            return -1;
        }

        public int getRotor(rotor r)
        {
            for (int j = 0; j < ROTORS.Length; j++)
                if (r == ROTORS[j])
                    return j;
            return -1;
        }

        public string getRotors() {
            string a = "";
            for (int i = 0; i < this.rotors.Length; i++) a += getRotor(this.rotors[i]) + ", ";
            return a;
        }

        public void addRotor(int n) {
            rotor[] a = new rotor[this.rotors.Length + 1];
            for(int i = 0; i < a.Length - 1; i++) a[i] = this.rotors[i];
            a[a.Length - 1] = ROTORS[n];
            this.rotors = a;
        }

        public void shift(int n_rotor) {
            this.rotors[n_rotor].i++;
            if (chr(this.rotors[n_rotor].i) == this.rotors[n_rotor].c && n_rotor != 0) {
                this.shift(n_rotor - 1);
                this.rotors[n_rotor].i %= this.rotors[0].alf.Length;
            }
        }

        public void shift(int n_rotor, char c) {
            this.rotors[n_rotor].i = ord(c);
        }

        public char crypt_char(char c) {
            if (!ROTORS[0].alf.Contains(c.ToString().ToLower())) return c;
            bool upper;
            if (Char.IsUpper(c)) {
                c = Char.ToLower(c);
                upper = true;
            } else {
                upper = false;
            }
            shift(this.rotors.Length - 1);
            int last = 0;
            Console.WriteLine(c);
            for (int i=this.rotors.Length - 1; i >= 0; i--) {
                c = this.rotors[i].alf[(ord(c) + this.rotors[i].i - last + 26) % 26];
                last = this.rotors[i].i;
                Console.WriteLine(c + " " + last);
            }
            c = this.reflec.alf[(ord(c) - last + 26) % 26];
            Console.WriteLine("3. " + c + " " + last);
            last = 0;
            for (int i = 0; i < this.rotors.Length; i++) {
                c = chr(this.rotors[i].alf.IndexOf(chr(ord(c) + this.rotors[i].i - last)));
                last = this.rotors[i].i;
                Console.WriteLine(c + " " + last);
            }
            c = chr(ord(c) - last);
            if (upper) return Char.ToUpper(c);
            return c;
        }
    }

    class Program {
        static void Main(string[] args) {
            enigma a = new enigma(new int[] {1, 2, 3}, 1);
            Console.WriteLine(a.getRotors());
            char ch;
            ch = Console.ReadLine()[0];
            Console.WriteLine(a.crypt_char(ch));
            Console.ReadKey();
        }
    }
}
