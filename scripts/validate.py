import hashlib
from glob import glob
import pathlib, re, json, sys


## Constants
CURRENT_DIR = pathlib.Path(__file__).parent.resolve()
CHECKSUM_FILEPATH = r'scripts/checksums.json'
FILECOUNT = 6


## Helper functions
def GetSavedChecksums(path: str = CHECKSUM_FILEPATH) -> dict:
    content = None
    with(open(path, mode='r', encoding='utf-8')) as file:
        content = json.loads(file.read())
    return content

def SaveChecksumJson(values: str, path: str = CHECKSUM_FILEPATH):
    with(open(path, mode='w+', encoding='utf-8')) as file:
        file.write(json.dumps(values, indent=4, ensure_ascii=False))

def GetFileNameByPath(file: str) -> str:
    pattern = r'[0-9]{6,9}\.(?:png|jpeg|gif|webm|jpg)'
    return re.search(pattern, file).group()


## Initial saving of checksums
def GenerateAndSafeFileChecksums() -> list[str]:
    files = glob(f'{CURRENT_DIR}/files/*.*')
    dict = {}

    for file in files:
        filename = GetFileNameByPath(file)
        checksum = GetChecksumFromFile(file)
        dict[filename] = checksum

    SaveChecksumJson(dict)
    return files


## Generate checksum from file
def GetChecksumFromFile(path: str, algo: str = "sha256", chunk_size: int = 1024 * 1024):
    h = hashlib.new(algo)
    with open(path, "rb") as f:
        for chunk in iter(lambda: f.read(chunk_size), b""):
            h.update(chunk)
    return h.hexdigest()

def ChecksumValid(file: str, checksums: dict) -> bool:
    res = GetChecksumFromFile(file) == checksums[GetFileNameByPath(file)]

    if(res):
        print(f'Integretiy of file \'{file}\' verified.')
    else:
        print(f'Integretiy of file \'{file}\' could NOT be verified!')

    return res

def ValidateFiles(ImagePath: str) -> int:
    files = glob(f'{ImagePath}/*')

    if(len(files) != FILECOUNT):
        return 1
    
    checksums = GetSavedChecksums()
    areAllValid = all(ChecksumValid(file, checksums) for file in files)

    print(f'Validation completed; Status: {'OK' if areAllValid else 'NOT OK'}')
    return 0 if areAllValid else 1


# files = GenerateAndSafeFileChecksums()
sys.exit(ValidateFiles(sys.argv[1]))