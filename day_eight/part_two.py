'''test = [
    '30373',
    '25512',
    '65332',
    '33549',
    '35390'
    ]'''

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
    highest_count = 0

    north = 0
    south = 0
    east = 0
    west = 0



    for i in range(1, len(test) - 1):
        for j in range(1, len(test[0]) - 1):
            candidate_tree = int(test[i][j])
            #before i
            for k in reversed(range(0, i)):
                if candidate_tree > int(test[k][j]):
                    north += 1
                if candidate_tree <= int(test[k][j]):
                    north += 1                   
                    break
                
            #after i       
            for k in range(i + 1, len(test)):
                if candidate_tree > int(test[k][j]):
                    south += 1
                if candidate_tree <= int(test[k][j]):
                    south += 1
                    break

            # before j   
            for k in reversed(range(0, j)):
                if candidate_tree > int(test[i][k]):
                    west += 1
                if candidate_tree <= int(test[i][k]):
                    west += 1
                    break
    
            #after j           
            for k in range(j + 1, len(test[0])):
                if candidate_tree > int(test[i][k]):
                    east += 1
                if candidate_tree <= int(test[i][k]):
                    east += 1
                    break
    
            count = north * south * west * east
            
            if count > highest_count:
                highest_count = count

            north = 0
            south = 0
            east = 0
            west = 0
            




    print(highest_count)
                        
