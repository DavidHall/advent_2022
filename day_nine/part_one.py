# two functions: create grid, and map movements
def main():
        
    '''
    data = [
        'R 4',
        'U 4',
        'L 3',
        'D 1',
        'R 4',
        'D 1',
        'L 5',
        'R 2'
    ]
    '''

    with open('input.txt') as file:
        data = file.readlines()


        tail_movements = register_head(data)

        print(len(tail_movements))



def register_head(data):

    test = [line.split() for line in data]

    head_x = 100
    head_y = 100
    tail_x = 100
    tail_y = 100

    tail = [[100, 100]]

    for line in test:
        if line[0] == 'R':
            for i in range(int(line[1])):
                head_x += 1
                if tail_y in (head_y + 1, head_y -1) and head_x > tail_x + 1:
                    tail_y = head_y
                if head_x > tail_x:
                    tail_x = head_x - 1
                position = [tail_x, tail_y]
                if position not in tail:
                    tail.append(position)
            
        if line[0] == 'L':
            for i in range(int(line[1])):
                head_x -= 1
                if tail_y in (head_y + 1, head_y -1) and head_x < tail_x - 1:
                    tail_y = head_y
                if head_x < tail_x:
                    tail_x = head_x + 1
                position = [tail_x, tail_y]
                if position not in tail:
                    tail.append(position)

        if line[0] == 'U':
            for i in range(int(line[1])):
                head_y += 1
                if tail_x in (head_x + 1, head_x -1) and head_y > tail_y + 1:
                    tail_x = head_x
                if head_y > tail_y:
                    tail_y = head_y - 1
                position = [tail_x, tail_y]
                if position not in tail:
                    tail.append(position)
           
        if line[0] == 'D':
            for i in range(int(line[1])):
                head_y -= 1
                if tail_x in (head_x + 1, head_x -1) and head_y < tail_y - 1:
                    tail_x = head_x
                if head_y < tail_y:
                    tail_y = head_y + 1
                position = [tail_x, tail_y]
                if position not in tail:
                    tail.append(position)

        
    
    return tail

main()