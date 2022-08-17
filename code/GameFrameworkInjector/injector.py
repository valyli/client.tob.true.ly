
import logging
import logging.config
import os
import shutil
import sys
from typing import List, Dict

import yaml


def parse_cmd()->bool:
    print(sys.argv)
    if not len(sys.argv) == 3:
        return False, None, None
    return True, sys.argv[1], sys.argv[2]


def copy_file(src_path, dst_path, sub_path):
    src = '{}{}{}'.format(src_path, os.path.sep, sub_path)
    dst = '{}{}{}'.format(dst_path, os.path.sep, sub_path)
    logging.info('\t%s -> %s', src, dst)
    shutil.copy(src, dst)


def run(src_path, dst_path):
    logging.info('Inject from %s -> %s', src_path, dst_path)
    sub_dirs = ['Assets/GameFramework', 'Assets/GameMain']
    for i in range(len(sub_dirs)):
        src = '{}{}{}'.format(src_path, os.path.sep, sub_dirs[i])
        dst = '{}{}{}'.format(dst_path, os.path.sep, sub_dirs[i])
        logging.info('\t%s -> %s', src, dst)
        shutil.copytree(src, dst)

    game_launcher_scene_path = 'Assets/Game Launcher.unity'
    copy_file(src_path, dst_path, game_launcher_scene_path)

    # buildsetting_path = 'ProjectSettings/EditorBuildSettings.asset'
    # copy_file(src_path, dst_path, buildsetting_path)

    dst = '{}{}{}'.format(dst_path, os.path.sep, 'Assets/StreamingAssets')
    os.mkdir(dst)

    keep_file = 'Assets/StreamingAssets/.gitkeep'
    copy_file(src_path, dst_path, keep_file)



def read_file_string(file_path, ignore_line_numbers: List[int], ignore_line_contents: List[str]):
    txts = ''
    ignored_lines: Dict[int, str] = {}
    with open(file_path) as fp:
        ln = fp.readline()
        ln_count = 0
        while ln is not None and not ln == '':
            ignored = False
            if ln_count in ignore_line_numbers:
                ignored = True
            else:
                for ln_content in ignore_line_contents:
                    if ln_content in ln:
                        ignored = True
                        break
            if ignored:
                ignored_lines[ln_count] = ln
            else:
                txts += ln
            ln_count += 1
            ln = fp.readline()
    return txts, ignored_lines


def modify_buildsetting(src_path, dst_path):
    buildsetting_path = 'ProjectSettings/EditorBuildSettings.asset'
    file_path = '{}{}{}'.format(src_path, os.path.sep, buildsetting_path)
    txts = ''
    replace_lns: Dict[int, str] = {}

    with open(file_path) as fp:
        ln = fp.readline()
        ln_count = 0
        while ln is not None and not ln == '':
            if '--- !u!' in ln:
                replace_lns[ln_count] = ln
                ln = '---\n'
            txts += ln
            ln_count += 1
            ln = fp.readline()

    yaml_txt = yaml.load(txts, Loader=yaml.FullLoader)
    print(yaml_txt)
    yaml_txt['m_Scenes']


logging.config.fileConfig('./conf/logging.conf')
r, src_path, dst_path = parse_cmd()
if r:
    # modify_buildsetting(src_path, dst_path)
    run(src_path, dst_path)
else:
    logging.error('Command line format: python injector.py <src project path> <dst project path>')

