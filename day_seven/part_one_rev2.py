input = [
    '$ cd /',
    '$ ls',
    'dir a',
    '14848514 b.txt',
    '8504156 c.dat',
    'dir d',
    '$ cd a',
    '$ ls',
    'dir e',
    '29116 f',
    '2557 g',
    '62596 h.lst',
    '$ cd e',
    '$ ls',
    '584 i',
    '$ cd ..',
    '$ cd ..',
    '$ cd d',
    '$ ls',
    '4060174 j',
    '8033020 d.log',
    '5626152 d.ext',
    '7214296 k'
]

with open('input.txt') as file:
    input = file.readlines()


    split_lines = [x.split() for x in input]

    database = {}
    path = ''

    for line in split_lines:
        if line[0] == '$':
            if line[1] == 'ls':
                continue
            if line[2] == '..':
                path = path[:path.rfind('/')]
                continue
            if line[1] == 'cd': 
                if line[2] == '/':
                    path += '/home'
                    database.update({path: 0})
                    continue
                path += f'/{line[2]}'
                database.update({path: 0})
            
        elif line[0] == 'dir':
                continue      
        else:
            temp = path
            size = line[0]
            while temp != '':
                database[temp] += int(size)
                temp = temp[:temp.rfind('/')]

    total = 0
    for key in database:
        if database[key] <= 100000:
            total += database[key]

    print(total)
