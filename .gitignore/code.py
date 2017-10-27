ROTORS = [
    ['abcdefghijklmnopqrstuvwxyz', 'a'],  # alphabet
    ['ekmflgdqvzntowyhxuspaibrcj', 'r'],  # I rotor
    ['ajdksiruxblhwtmcqgznpyfvoe', 'f'],  # II rotor
    ['bdfhjlcprtxvznyeiwgakmusqo', 'w'],  # III rotor
    ['esovpzjayquirhxlnftgkdcmwb', 'k'],  # IV rotor
    ['vzbrgityupsdnhlxawmjqofeck', 'a'],  # V rotor
    ['jpgvoumfyqbenhzrdkasxlictw', 'a'],
    ['nzjhgrcxmyswboufaivlpekqdt', 'a'],
    ['fkqhtlxocbjspdzramewniuygv', 'a'],
    ['leyjvcnixwpbqmdrtakzgfuhos', 'a'],
    ['fsokanuerhmbtiycwlqpzxvgjd', 'a']
]

REFLECTORS = {
    'a': [['ejmzalyxvbwfcrquontspikhgd', None], 0],
    'b': [['yruhqsldpxngokmiebfzcwvjat', None], 0]
}

# rework built-in function
old_ord, old_chr = ord, chr


def ord(x):
    return old_ord(x) - 97


def chr(x):
    return old_chr(x % 26 + 97)


class enigma:
    '''crypt lower and upper english characters'''

    def __init__(self, *rotors, reflector='b', comutators=None):
        self.rotors = []
        for i in rotors:
            self.rotors.append([ROTORS[i], 0])
        self.comutators = comutators or {}
        self.reflector = REFLECTORS[reflector]

    def add_rotor(self, rotor, n=None):
        if n is None:
            self.rotors.append(rotor)
        else:
            self.rotors.insert(n, rotor)

    def del_rotor(self, n):
        del self.rotors[n]

    def add_comutator(self, comutatorA, comutatorB):
        self.comutators.update({comutatorA: comutatorB,
                                comutatorB: comutatorA})

    def del_comutator(self, comutatorA, comutatorB):
        del self.comutators[comutatorA], self.comutators[comutatorB]

    def edit_reflector(self, reflector):
        self.r = REFLECTORS[reflector]

    def get_key(self):
        return [self.rotors, self.comutators, self.r]

    def shift(self, n_rotor, n=0, letter=''):
        if n != 0:
            self.rotors[n_rotor][1] += n
            if chr(self.rotors[n_rotor][1]) == self.rotors[n_rotor][0][1]\
               and n_rotor != 0:
                self.shift(n_rotor - 1, 1)
                self.rotors[n_rotor][1] %= len(self.rotors[n_rotor][0][0])
        else:
            self.rotors[n_rotor][1] = ord(letter)

    def crypt_char(self, letter):
        if letter.lower() not in ROTORS[0][0]:
            return letter
        elif letter.isupper():
            letter, upper = letter.lower(), True
        else:
            upper = False
        self.shift(-1, 1)
        last = 0
        for i in list(reversed(self.rotors)) + [self.reflector]:
            letter = i[0][0][(ord(letter) + i[1] - last) % 26]
            last = i[1]
        for i in self.rotors:
            letter = chr(i[0][0].index(chr(ord(letter) + i[1] - last)))
            last = i[1]
        letter = chr(ord(letter) - last)
        if letter in self.comutators:
            letter = self.comutators[letter]
        if upper:
            return letter.upper()
        else:
            return letter

    def crypt_text(self, text):
        return ''.join(self.crypt_char(x) for x in text)


q = enigma(3, 2, 1)
q.shift(0, letter='c')
q.shift(1, letter='u')
q.shift(2, letter='q')
print(q.crypt_text('abcd'))
input()
