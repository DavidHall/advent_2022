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
    ]'''


    with open('input.txt') as file:
        lines = file.readlines()

        before = stack_and_instruction_parser(lines)
        after = rearranger(before)
        top_crates = get_top_crates(after)
        print(after)
        print(top_crates)
            #printer(after)

    

def stack_and_instruction_parser(input):
    instructions = []

    stacks = [None] * 9

    for index in range(9):
        stacks[index] = []

    for line in input:
        if line == '' or line[:2] == ' 1':
            continue
        new_line = line.replace('    ', ' ')
        crates = new_line.split(' ') 
        crates = [s.replace("\n", "") for s in crates]
        if crates[0] == 'move':
            instruction = []   
            instruction.extend([int(crates[1]), int(crates[3]) - 1, int(crates[5]) - 1])
            instructions.append(instruction)
            continue

        for index, crate in enumerate(crates):
            if crate == '':
                continue

            stacks[index].insert(0, crate[1])
        
        
        
    return [stacks, instructions]
    
def rearranger(input):
    stacks = input[0]
    instructions = input[1]

    for instruction in instructions:
        source = instruction[1]
        target = instruction[2]
        number_of_crates = instruction[0]
        crates = stacks[source][-number_of_crates:]
        stacks[source] = stacks[source][:-number_of_crates]
        stacks[target].extend(crates)

    return stacks

def get_top_crates(stacks):
    result = ""

    for stack in stacks:
        if len(stack) == 0:
            continue
        result += stack[-1]

    return result
        
main()