'''
collect three lines in a list
compare to find common element
-iterate thru chars of first line
-look for that char in second string:
if found go to secon string:
if match is found get priority value of char (if lower, then -96. if upper then - 38)
add to sum
next 3 lines



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
    count = 0
    group = []

    for line in lines:
        group.append(line)
        count += 1
        if count == 3:
                for item in group[0]:
                        if item in group[1]:
                                if item in group[2]:
                                    if item.islower():
                                        sum += (ord(item) - 96)
                                        count = 0
                                        group.clear()
                                        break
                                    if item.isupper():
                                        sum += (ord(item) - 38)
                                        count = 0
                                        group.clear()
                                        break  
            

    print(sum)