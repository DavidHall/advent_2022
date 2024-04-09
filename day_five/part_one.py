def main():

    '''lines = [ 
    '    [D]',    
    '[N] [C]',   
    '[Z] [M] [P]',
    ' 1   2   3',
    '',
    'move 1 from 2 to 1',
    'move 3 from 1 to 3',
    'move 2 from 2 to 1',
    'move 1 from 1 to 2'
    ]
'''
    with open('input.txt') as file:
        lines = file.readlines()

        before = stack_and_instruction_parser(lines)
        after = rearranger(before)
        printer(after)

    

def stack_and_instruction_parser(input):
    stack_one = []
    stack_two = []
    stack_three = []
    stack_four = []
    stack_five = []
    stack_six = []
    stack_seven = []
    stack_eight = []
    stack_nine = []
    instructions = []
    for line in input:
        if line == '' or line[:2] == ' 1':
            continue
        new_line = line.replace('    ', ' ')
        crates = new_line.split(' ') 
        crates = [s.replace("\n", "") for s in crates]
        if crates[0] == 'move':
            instruction = []   
            instruction.extend([crates[1], crates[3], crates[5]])
            instructions.append(instruction)
            continue
        for index, crate in enumerate(crates):
            if crate == '':
                continue
            if index == 0:
                stack_one.insert(0, crate[1])
            if index == 1:
                stack_two.insert(0, crate[1])
            if index == 2:
                stack_three.insert(0, crate[1])
            if index == 3:
                stack_four.insert(0, crate[1])
            if index == 4:
                stack_five.insert(0, crate[1])
            if index == 5:
                stack_six.insert(0, crate[1])
            if index == 6:
                stack_seven.insert(0, crate[1])
            if index == 7:
                stack_eight.insert(0, crate[1])
            if index == 8:
                stack_nine.insert(0, crate[1])


    return [stack_one, stack_two, stack_three, stack_four, stack_five, stack_six, stack_seven, stack_eight, stack_nine, instructions]
    
def rearranger(input):
    one = input[0]
    two = input[1]
    three = input[2]
    four = input[3]
    five = input[4]
    six = input[5]
    seven = input[6]
    eight = input[7]
    nine = input[8]
    instructions = input[9]

    for seq in instructions:
        source = seq[1]
        target = seq[2]
        number_of_crates = seq[0]
        for i in range(int(number_of_crates)):
            if source == '1':
                crate = one.pop()
            if source == '2':
                crate = two.pop()
            if source == '3':
                crate = three.pop()
            if source == '4':
                crate = four.pop()
            if source == '5':
                crate = five.pop()
            if source == '6':
                crate = six.pop()
            elif source == '7':
                crate = seven.pop()
            if source == '8':
                crate = eight.pop()
            if source == '9':
                crate = nine.pop()

            if target == '1':
                one.append(crate)
            if target == '2':
                two.append(crate)
            if target == '3':
                three.append(crate)
            if target == '4':
                four.append(crate)
            if target == '5':
                five.append(crate)
            if target == '6':
                six.append(crate)
            if target == '7':
                seven.append(crate)
            if target == '8':
                eight.append(crate)
            if target == '9':
                nine.append(crate)

    return [one, two, three, four, five, six, seven, eight, nine]

def printer(input):
    
    top_crates = ''.join([crate[-1] for crate in input])
    
    print(top_crates)

main()