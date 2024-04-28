'''test = [
    '30373',
    '25512',
    '65332',
    '33549',
    '35390'
    ]
'''
'''test = [
    '6477',
    '8302',
    '2859'
]'''
#1,1
'''0,1
Loop A 1,0
LOOP B 1,2 , 1,3
2,1

0,2
Loop A 1,0 , 1,1
 Loop B 1,3
2,2'''

with open('input.txt') as file:
    temp = file.readlines()
    test = [line.rstrip('\n') for line in temp]


    count = 0
    N = True
    S = True
    E = True
    W = True

    for i in range(1, len(test) - 1):
        for j in range(1, len(test[0]) - 1):
            candidate_tree = int(test[i][j])
            #before i
            for k in range(0, i):
                if candidate_tree <= int(test[k][j]):
                    N = False
                    break
                        
            #after i
            if N == False:
                for k in range(i + 1, len(test)):
                    if candidate_tree <= int(test[k][j]):
                        S = False
                        break

                        
            # before j
            if S == False and N == False:
                for k in range(0, j):
                    if candidate_tree <= int(test[i][k]):
                        W = False
                        break
            #after j           
            if S == False and N == False and W == False:
                for k in range(j + 1, len(test[0])):
                    if candidate_tree <= int(test[i][k]):
                        E = False
                        break

            if N == True or S == True or E == True or W == True:
                count += 1
            
            N = True
            S = True
            E = True
            W = True



    count += (len(test) * 2) + (len(test[0]) * 2) - 4
    print(count)
                        
