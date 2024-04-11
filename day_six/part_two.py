
#input = 'mjqjpqmgbljsphdztnvjfqwrcgsmlb'

with open('input.txt') as file:
    lines = file.readlines()
    input = lines[0]
    counter = 0

    for i in range(len(input)):
        counter += 1
        marker = set(char for char in input[i:i+14])
        if len(marker) == 14:
            print(counter + 13)
            break