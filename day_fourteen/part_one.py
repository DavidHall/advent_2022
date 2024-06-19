x_offset = 494

input = [
    '498,4 -> 498,6 -> 496,6',
    '503,4 -> 502,4 -> 502,9 -> 494,9'
]

data = []


cave = [['.' for i in range(10)] for i in range(10)]



'''
[
    [(498, 4), (498, 6), (496, 6)], 
    [(503, 4), (502, 4), (502, 9), (494, 9)]
]
'''

def create_windowed(list):
    result = []

    for i in range(len(list) -1):
        result.append((list[i],list[i+1]))

    return result

for line in input:
    l = []
    numbers = line.split(' -> ')
    for number in numbers:
        coordinates = number.split(',')
        l.append((int(coordinates[0]) - x_offset,  int(coordinates[1])))
    
    data.append(create_windowed(l))

def swap(a, b):
    if (b < a):
        return (b, a)
    return (a, b)

for scan1 in data:
    for point in scan1:
        first = point[0]
        second = point[1]

        if first[0] == second[0]:
            swapped = swap(first[1], second[1])
            for i in range(swapped[0], swapped[1] + 1):
                cave[i][first[0]] = '#'
        else:
            swapped = swap(first[0], second[0])
            for i in range(swapped[0], swapped[1] + 1):
                cave[first[1]][i] = '#'





#falling sand.


cave[0][6] = 'o'

y = 0
x = 6
grains_of_sand = 0
while y < 10 and (-1 < x < 10):
    
    #cave[y][x] == 'o'
    for slice in cave:
            print(slice)
    if cave[y+1][x] == '.':
        cave[y][x] = '.'
        cave[y+1][x] = 'o'
        y = y+1
        continue
        
    if cave[y+1][x] == '#' or cave[y+1][x] == 'o':
        if x-1 == -1:
            cave[y][x] = '.'
            break
        if cave[y+1][x-1] == '.':
            cave[y][x] = '.'
            cave[y+1][x-1] = 'o'
            y = y+1
            x = x-1
            continue

        

        if cave[y+1][x-1] == '#' or cave[y+1][x-1] == 'o':
            if cave[y+1][x+1] == '.':
                cave[y][x] = '.'
                cave[y+1][x+1] = 'o'
                y = y+1
                x = x+1
                continue

            if cave[y+1][x+1] == '#' or cave[y+1][x+1] == 'o':
                y = 0
                x = 6
                grains_of_sand += 1
                continue
            
for slice in cave:
    print(slice)    
print(grains_of_sand)
