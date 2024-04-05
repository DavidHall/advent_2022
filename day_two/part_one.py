
'''
A = ('opponent', 'rock', 1)
B = ('opponent', 'paper', 2)
C = ('opponent', 'scissors', 3)

X = ('me', 'rock', 1)
Y = ('me', 'paper', 2)
Z = ('me', 'scissors', 3)

A == X
B == Y
C == Z
A > Z 
A < Y
B > X
B < Z
C > Y
C < X
'''

'''
file = [
    'A Y',
    'B X',
    'C Z'
]
'''

with open('input.txt') as file:
    lines = file.readlines()



    lose = 0
    draw = 3
    win = 6

    score = 0

    for line in lines:
        if line[0] == 'A':
            if line[2] == 'X':
                score += 1 + draw
            if  line[2] == 'Y':
                score += 2 + win
            if line[2] == 'Z':
                score += 3 + lose
        if line[0] == 'B':
            if line[2] == 'X':
                score += 1 + lose
            if  line[2] == 'Y':
                score += 2 + draw
            if line[2] == 'Z':
                score += 3 + win
        if line[0] == 'C':
            if line[2] == 'X':
                score += 1 + win
            if  line[2] == 'Y':
                score += 2 + lose
            if line[2] == 'Z':
                score += 3 + draw


    print(score)
