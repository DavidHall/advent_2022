
'''
A = ('opponent', 'rock', 1)
B = ('opponent', 'paper', 2)
C = ('opponent', 'scissors', 3)



A == X
B == Y
C == Z
A > Z 
A < Y
B > X
B < Z
C > Y
C < X




file = [
    'A Y',
    'B X',
    'C Z'
]
X = ('me', 'lose', 0)
Y = ('me', 'draw', 3)
Z = ('me', 'win', 6)
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
                score += 3 + lose
            if  line[2] == 'Y':
                score += 1 + draw
            if line[2] == 'Z':
                score += 2 + win
        if line[0] == 'B':
            if line[2] == 'X':
                score += 1 + lose
            if  line[2] == 'Y':
                score += 2 + draw
            if line[2] == 'Z':
                score += 3 + win
        if line[0] == 'C':
            if line[2] == 'X':
                score += 2 + lose
            if  line[2] == 'Y':
                score += 3 + draw
            if line[2] == 'Z':
                score += 1 + win


    print(score)
