'''
map priority values for items?... ascii val - (lc=96, UC=38)
divide each line in two
compare to find common element
-iterate thru chars of first string
-look for that char in second string
-if match is found get priority value of char (if lower, then -96. if upper then - 38)
add to sum
next line



lines = [
    'vJrwpWtwJgWrhcsFMMfFFhFp',
    'jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL',
    'PmmdzqPrVvPwwTWBwg',
    'wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn',
    'ttgJtRGJQctTZtZT',
    'CrZsJsPPZsGzwwsLwLmpwMDw',
]
'''
with open('input.txt') as file:
    lines = file.readlines()

    sum = 0

    for line in lines:
        half = (len(line) // 2)
        compartment1 = line[:half]
        compartment2 = line[half:]
        for char in compartment1:
            if char in compartment2:
                if char.islower():
                    sum += (ord(char) - 96)
                    break
                if char.isupper():
                    sum += (ord(char) - 38)
                    break

    print(sum)