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

'''with open('input.txt') as file:
    input = file.readlines()'''


split_lines = [x.split() for x in input]

database = {}
head = database
history = []
history_as_string = 'head = database'



for line in split_lines:
    if line[1] == 'cd':
        if line[2] in head:
            history.append(line[2])
            history_as_string = 'head = database'
            for item in history:
                history_as_string += f'["{item}"]'
                exec(history_as_string)
        elif line[2] == '..':
            history = history[:-1]
            history_as_string = 'head = database'
            for item in history:
                history_as_string += f'["{item}"]'
                exec(history_as_string)
        else:
            #exec(history_as_string)
            head.update({line[2]: {}})
            history.append(line[2])
            history_as_string = 'head = database'
            for item in history:
                history_as_string += f'["{item}"]'
        
        
        
    if line[1] == 'ls':
        continue
    if line[0] == 'dir':
        exec(history_as_string)
        head.update({line[1]: {}})
        #head = database[head]
    if line[0] not in ('$', 'dir'):
        head.update({line[1]: line[0]})

summed_dirs = {}


def list_items(database, path):
    sum = 0
    #path += '/'
    summed_dirs.update({path: 0})
    temp = path
   
    for key in database:    
        if type(database[key]) == str:
            num = int(database[key])
            sum += num

    while temp not in ['/', '']:
        summed_dirs[temp] += sum
        temp = temp[:temp.rfind('/')]

    for key in database:    
        if type(database[key]) == dict:
            if key == '/':
                path = '/home'
                list_items(database[key], path)
            else:
                path += f'/{key}'
                list_items(database[key], path)
        

    

    
    
    
  
        

list_items(database, '/home')
total = 0
for key in summed_dirs:
    if summed_dirs[key] <= 100000:
        total += summed_dirs[key]
print(total)
#total = sum(item_list)
#print(total)
