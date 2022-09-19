
import logging
import logging.config
import os
import shutil
import sys
from typing import List, Dict


def parse_cmd()->bool:
    print(sys.argv)
    if not len(sys.argv) == 3:
        return False, None, None, None
    return True, sys.argv[1], sys.argv[2], sys.argv[3]


def read_file_string(file_path):
    txts = ''
    with open(file_path) as fp:
        ln = fp.readline()
        while ln is not None and not ln == '':
            txts += ln
            ln = fp.readline()
    return txts


def find_build_report_info(build_report_path, _platform):
    # [17:12:41.534][INFO] Process updatable version list for 'Android' complete, updatable version list path is 'D:/output/StarForceAssetBundle/Full/0_1_0_13/Android/GameFrameworkVersion.cb71efbe.dat', length is '7353', hash code is '-881725506[0xCB71EFBE]', compressed length is '2754', compressed hash code is '-2103068400[0x82A5B910]'.
    with open(build_report_path, 'r') as fp:

    return VersionListLength, VersionListHashCode, VersionListCompressedLength, VersionListCompressedHashCode

def run(output_root_path, project_name, _version, _platform):
    # Example:
    # output_root_path = D:\output
    # project_path = D:\output\StarForceAssetBundle
    # full_path = D:\output\StarForceAssetBundle\Full
    # resource_path = D:\output\StarForceAssetBundle\Full\0_1_0_13
    # build_report_path = D:\output\StarForceAssetBundle\BuildReport\0_1_0_13\BuildLog.txt
    project_path = '{}{}{}'.format(output_root_path, os.path.sep, project_name)
    full_path = '{}{}Full'.format(project_path, os.path.sep)
    resource_path = '{}{}{}'.format(full_path, os.path.sep, _version)
    platform_path = '{}{}{}'.format(resource_path, os.path.sep, _platform)
    build_report_path = '{}{}BuildReport{}{}{}BuildLog.txt'.format(project_path, os.path.sep, os.path.sep, _version, os.path.sep)

    VersionListLength, VersionListHashCode, VersionListCompressedLength, VersionListCompressedHashCode = find_build_report_info(build_report_path, _platform)

    aaa = 1



logging.config.fileConfig('./conf/logging.conf')
r, output_root_path, project_name, _version, _platform = parse_cmd()

if r:
    run(output_root_path, project_name, _version, _platform)
else:
    logging.error('Command line format: python injector.py <src project path> <dst project path>')

